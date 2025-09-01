//This is example of usage BCF engine in TypeScript.
//  ExampleRawBCF for raw API usage
//  ExampleBCFWrapper for usage of string convince API
//
//See BCFModule.ts and BCFModuleWrapper.ts for API definitions
//
//ensure you did npm install--save - dev typescript
//
// 

import { readFile } from "fs/promises";
import { BCFModuleWrapper } from "./BCFModuleWrapper.js";

const BCF_FILE_PATH = "W:\\DevArea\\buildingSMART\\BCF-XML\\Test Cases\\v3.0\\Visualization\\Orthogonal camera\\orthogonal camera.bcf";

//
//
async function ExampleRawBCF() {

    console.log("ExampleRawBCF");
    // Dynamically import the Emscripten JS module
    const initBcfEngine: any = (await import("./RDF.BCF.js")).default; 
    const Module: BCFModule = await initBcfEngine();
    console.log("BCF module initialized:", Module);

    // Create a new BCF project
    const bcfData = Module._bcfProjectCreate(0);
    console.log("BCF data pointer:", bcfData);

    // Load BCF file into Emscripten FS
    const fileContent = await readFile(BCF_FILE_PATH);
    const fileData = new Uint8Array(await fileContent);
    Module.FS.writeFile(BCF_FILE_PATH, fileData);

    // Allocate memory for file path
    const pathPtr = Module._malloc(BCF_FILE_PATH.length + 1);
    Module.stringToUTF8(BCF_FILE_PATH, pathPtr, BCF_FILE_PATH.length + 1);

    // Call the C function
    let ok = Module._bcfFileRead(bcfData, pathPtr, true);
    console.log("File loaded:", ok);

    // Get project ID
    const idPtr = Module._bcfProjectIdGet(bcfData);
    const idStr = Module.UTF8ToString(idPtr);
    console.log("Project ID:", idStr);

    // Close project
    ok = Module._bcfProjectDelete(bcfData);
    console.log("BCF data cleaned: ", ok);

    // Free memory
    Module._free(pathPtr);
    console.log("exit ExampleRawBCF");
}

//
//
async function ExampleBCFWrapper() {

    console.log("ExampleBCFWrapper");

    // Dynamically import the Emscripten JS module
    const initBcfEngine: any = (await import("./RDF.BCF.js")).default;
    const Module: BCFModule = await initBcfEngine();
    console.log("BCF module initialized:", Module);
    const bcf = new BCFModuleWrapper(Module);

    // Use
    const bcfData = bcf.bcfProjectCreate("MyProject");
    console.log("BCF data pointer:", bcfData);

    // Load BCF file into Emscripten FS
    const fileContent = await readFile(BCF_FILE_PATH);
    const fileData = new Uint8Array(await fileContent);
    Module.FS.writeFile(BCF_FILE_PATH, fileData);

    let ok = bcf.bcfFileRead(bcfData, BCF_FILE_PATH, true);
    console.log("File read:", ok);

    const projId = bcf.bcfProjectIdGet(bcfData);
    console.log("Project ID:", projId);

    ok = bcf.bcfProjectDelete(bcfData);
    console.log("Cleanup:", ok);

    console.log("Exit ExampleBCFWrapper");
}

//
//
async function main() {
    await ExampleRawBCF();
    await ExampleBCFWrapper();
}

main().catch(console.error);
