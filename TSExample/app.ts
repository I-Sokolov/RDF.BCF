//This is example of usage BCF engine in TypeScript.
//  ExampleRawBCF for raw API usage
//  ExampleBCFWrapper for usage of string convince API
//
//See BCFModule.ts and BCFModuleWrapper.ts for API definitions
//
//ensure you did npm install--save - dev typescript
//
// 

import { readFile, writeFile } from "fs/promises";
import { BCFModuleWrapper } from "./BCFModuleWrapper.js";

const BCF_FILE_PATH = "W:\\DevArea\\buildingSMART\\BCF-XML\\Test Cases\\v3.0\\Visualization\\Orthogonal camera\\orthogonal camera.bcf";
const BCF_FILE_PATH_SAVE = "W:\\DevArea\\WriteTest.bcf";

// Load BCF file into Emscripten FS
async function LoadFileToEMS(module: BCFModule, filePath: string) {``
    const fileContent = await readFile(filePath);
    const fileData = new Uint8Array(await fileContent);
    module.FS.writeFile(filePath, fileData);
    console.log(`File ${filePath} loaded into Emscripten FS`);
}

async function DownloadFileFromEMS(module: BCFModule, filePath: string) {
    const fileData = module.FS.readFile(filePath);
    await writeFile(filePath, fileData);
    console.log(`File ${filePath} downloaded from Emscripten FS`);
}

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

    //
    await LoadFileToEMS(Module, BCF_FILE_PATH);

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
    await LoadFileToEMS(Module, BCF_FILE_PATH);

    let ok = bcf.bcfFileRead(bcfData, BCF_FILE_PATH, true);
    console.log("File read:", ok);

    const projId = bcf.bcfProjectIdGet(bcfData);
    console.log("Project ID:", projId);

    ok = bcf.bcfFileWrite(bcfData, BCF_FILE_PATH_SAVE, 30);
    console.log("Write file:", ok);

    await DownloadFileFromEMS(Module, BCF_FILE_PATH_SAVE);

    ok = bcf.bcfProjectDelete(bcfData);
    console.log("Cleanup:", ok);

    console.log("Exit ExampleBCFWrapper");
}

//
//
async function TopicWithSnapshot()
{
    console.log("Topic With Snapshot Example");

    // Dynamically import the Emscripten JS module
    const initBcfEngine: any = (await import("./RDF.BCF.js")).default;
    const Module: BCFModule = await initBcfEngine();
    console.log("BCF module initialized:", Module);
    const bcf = new BCFModuleWrapper(Module);

    // 
    const bcfData = bcf.bcfProjectCreate("MyProject");
    console.log("BCF data pointer:", bcfData);

    bcf.bcfSetOptions(bcfData, "user@company.org", true);

    //
    let topic = bcf.bcfTopicAdd(bcfData, "MyTopic", "MyTopicTitle", "MyTopicDescription");
    let guid = bcf.bcfTopicGetGuid(topic);
    console.log("Added topic guid:", guid);

    let viewpoint = bcf.bcfViewPointAdd(topic);
    guid = bcf.bcfViewPointGetGuid(viewpoint);
    console.log("Added viewpoint guid:", guid);

    //add snapshot
    const snapshotFile = "..\\TestCases\\Architectural.png";
    await LoadFileToEMS(Module, snapshotFile);

    let ok = bcf.bcfViewPointSetSnapshot(viewpoint, snapshotFile);
    console.log("Set snapshot:", ok);

    //add required viewpoint properties
    bcf.bcfViewPointSetCameraViewPoint(viewpoint, { x: 10, y: 10, z: 10 });
    bcf.bcfViewPointSetCameraDirection(viewpoint, { x: 1, y: 0, z: 0 });
    bcf.bcfViewPointSetCameraUpVector(viewpoint, { x: 0, y: 1, z: 0 });
    bcf.bcfViewPointSetAspectRatio(viewpoint, 0.45);
    bcf.bcfViewPointSetFieldOfView(viewpoint, 33);

    //
    ok = bcf.bcfFileWrite(bcfData, "TopicWithSnapshotExample.bcf", 30);
    console.log("Write file:", ok);

    if (ok) {
        await DownloadFileFromEMS(Module, "TopicWithSnapshotExample.bcf");
    }
    
    ok = bcf.bcfProjectDelete(bcfData);
    console.log("Cleanup:", ok);

    console.log("Exit Topic With Snapshot Example");

}

//
//
async function main() {

    //await ExampleRawBCF();

    //await ExampleBCFWrapper();

    await TopicWithSnapshot();
}

main().catch(console.error);
