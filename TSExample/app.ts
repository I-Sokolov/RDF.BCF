//This is example of usage BCF engine in TypeScript.
//  ExampleRawBCF for raw API usage
//  ExampleBCFWrapper for usage of string convince API
//
//See BCFModule.ts and BCFModuleWrapper.ts for API definitions
//
//ensure you did npm install--save - dev typescript
//
// 

import { readFile, writeFile, mkdir} from "fs/promises";
import { BCFModuleWrapper } from "./BCFModuleWrapper.js";

const BCF_FILE_PATH = "W:\\DevArea\\buildingSMART\\BCF-XML\\Test Cases\\v3.0\\Visualization\\Orthogonal camera\\orthogonal camera.bcf";
const BCF_FILE_PATH_SAVE = "W:\\DevArea\\WriteTest.bcf";
const JAPANISE_TEST = "こんにちは、田中さん。";   // \u3053\u3093\u306B\u3061\u306F\u3001\u7530\u4E2D\u3055\u3093\u3002

function ASSERT(condition: boolean, message: string = "unexpected result") {
    if (!condition) {
        throw new Error("Assertion failed: " + message);
    }
}

// Dynamically import the Emscripten JS module
async function LoadBCFModule(): Promise<BCFModule> {
    const initBcfEngine: any = (await import("../emscripten/RDF.BCF.js")).default;
    const Module: BCFModule = await initBcfEngine();
    console.log("BCF module initialized:", Module);
    return Module;
}

function mkdirRecursive(module: BCFModule, path: string) {
    const parts = path.split('/').filter(p => p);
    let currentPath = '';

    for (let i = 0; i < parts.length - 1; i++) {
        const part = parts[i];
        currentPath += '/' + part;
        try {
            module.FS.mkdir(currentPath);
        } catch (e: any) {
            if (e.code !== 'EEXIST') throw e;
        }
    }
}

// Load BCF file into Emscripten FS
async function LoadFileToEMS(module: BCFModule, filePath: string, targetPath: string = filePath) {

    const fileContent = await readFile(filePath);
    const fileData = new Uint8Array(await fileContent);

    mkdirRecursive(module, targetPath);

    module.FS.writeFile(targetPath, fileData);
    console.log(`File ${filePath} loaded into Emscripten FS as ${targetPath}`);
}

async function DownloadFileFromEMS(module: BCFModule, filePath: string) {
    const fileData = module.FS.readFile(filePath);
    await writeFile(filePath, fileData);
    console.log(`File ${filePath} downloaded from Emscripten FS`);
}

function PrintFile(module: BCFModule, filePath: string) {
    const fileData = module.FS.readFile(filePath);
    const text = new TextDecoder("utf-8").decode(fileData);
    console.log("-------------------------------------------------------");
    console.log(text);
    console.log("-------------------------------------------------------");
}

//
//
async function ExampleRawBCF() {

    console.log("ExampleRawBCF");

    const Module = await LoadBCFModule();

    // Create a new BCF project
    const bcfData = Module._bcfProjectCreate(0);
    ASSERT(bcfData !== 0, "Failed to create BCF project");
    console.log("BCF data pointer:", bcfData);

    //
    await LoadFileToEMS(Module, BCF_FILE_PATH);

    // Allocate memory for file path
    const pathPtr = Module._malloc(BCF_FILE_PATH.length + 1);
    Module.stringToUTF8(BCF_FILE_PATH, pathPtr, BCF_FILE_PATH.length + 1);

    // Call the C function
    let ok = Module._bcfFileRead(bcfData, pathPtr, true);
    ASSERT(ok, "Failed to read BCF file");
    console.log("File loaded:", ok);

    // Get project ID
    const idPtr = Module._bcfProjectIdGet(bcfData);
    const idStr = Module.UTF8ToString(idPtr);
    console.log("Project ID:", idStr);

    // Close project
    ok = Module._bcfProjectDelete(bcfData);
    ASSERT(ok, "Failed to delete BCF project");
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
    const Module = await LoadBCFModule();
    const bcf = new BCFModuleWrapper(Module);

    // Use
    const bcfData = bcf.bcfProjectCreate("MyProject");
    ASSERT(bcfData !== 0, "Failed to create BCF project");
    console.log("BCF data pointer:", bcfData);

    // Load BCF file into Emscripten FS
    await LoadFileToEMS(Module, BCF_FILE_PATH);

    let ok = bcf.bcfFileRead(bcfData, BCF_FILE_PATH, true);
    ASSERT(ok, "Failed to read BCF file");
    console.log("File read:", ok);

    const projId = bcf.bcfProjectIdGet(bcfData);
    ASSERT(projId !== "", "Failed to get project ID");
    console.log("Project ID:", projId);

    ok = bcf.bcfFileWrite(bcfData, BCF_FILE_PATH_SAVE, 30);
    ASSERT(ok, "Failed to write BCF file");
    console.log("Write file:", ok);

    await DownloadFileFromEMS(Module, BCF_FILE_PATH_SAVE);

    ok = bcf.bcfProjectDelete(bcfData);
    ASSERT(ok, "Failed to delete BCF project");
    console.log("Cleanup:", ok);

    console.log("Exit ExampleBCFWrapper");
}

//
//
function CheckBCFProject(module: BCFModule, bcf: BCFModuleWrapper, project: number, topicGuid: string, viewPointGiud: string)
{
    let topicInd = 0;
    let topic = 0;
    while (topic = bcf.bcfTopicGetAt(project, topicInd)) {

        const guid = bcf.bcfTopicGetGuid(topic);
        const type = bcf.bcfTopicGetTopicType(topic);
        const title = bcf.bcfTopicGetTitle(topic);
        const status = bcf.bcfTopicGetTopicStatus(topic);

        console.log(`Topic ${topicInd}: guid=${guid}, type=${type}, title=${title}, status=${status}`);

        ASSERT(guid === topicGuid);
        ASSERT(type == "MyTopic " + JAPANISE_TEST);
        ASSERT(title === "MyTopicTitle " + JAPANISE_TEST);
        ASSERT(status == "MyTopicStatus" + JAPANISE_TEST); 

        const vp = bcf.bcfViewPointGetAt(topic, 0);
        const vpGuid = bcf.bcfViewPointGetGuid(vp);
        ASSERT(vpGuid === viewPointGiud);

        const snapshot = bcf.bcfViewPointGetSnapshot(vp);
        const snapShotFileData = module.FS.readFile(snapshot);
        ASSERT(snapShotFileData.length > 0);

        topicInd++;
    }
    ASSERT(topicInd == 1);
}

//
//
function CheckBCFFile(module: BCFModule, bcf: BCFModuleWrapper, filePath: string, topicGuid: string, viewPointGiud: string) {

    const project = bcf.bcfProjectCreate();

    let ok = bcf.bcfFileRead(project, filePath);
    ASSERT(ok, "Failed to read BCF file");
    console.log("Read BCF file: ", filePath, " result ", ok);

    CheckBCFProject(module, bcf, project, topicGuid, viewPointGiud);

    ok = bcf.bcfProjectDelete(project);
    ASSERT(ok, "Failed to delete BCF project");
    console.log("Close BCF: ", ok);
}

//
//
async function TopicWithSnapshot()
{
    console.log("Topic With Snapshot Example");

    // Dynamically import the Emscripten JS module
    const Module = await LoadBCFModule();
    const bcf = new BCFModuleWrapper(Module);

    // 
    const bcfData = bcf.bcfProjectCreate("MyProject");
    ASSERT(bcfData !== 0, "Failed to create BCF project");
    console.log("BCF data pointer:", bcfData);

    bcf.bcfSetOptions(bcfData, "user@company.org", true);

    //
    const topic = bcf.bcfTopicAdd(bcfData, "MyTopic " + JAPANISE_TEST, "MyTopicTitle " + JAPANISE_TEST, "MyTopicStatus" + JAPANISE_TEST);
    const topicGuid = bcf.bcfTopicGetGuid(topic);
    console.log("Added topic guid:", topicGuid);
    
    const viewpoint = bcf.bcfViewPointAdd(topic);
    const viewPointGuid = bcf.bcfViewPointGetGuid(viewpoint);
    console.log("Added viewpoint guid:", viewPointGuid);

    //add snapshot
    const snapshotFile = "/MyTest/test/Architectural.png";
    await LoadFileToEMS(Module, "../TestCases/Architectural.png", snapshotFile);

    let ok = bcf.bcfViewPointSetSnapshot(viewpoint, snapshotFile);
    console.log("Set snapshot:", ok);

    //add required viewpoint properties
    bcf.bcfViewPointSetCameraViewPoint(viewpoint, { x: 10, y: 10, z: 10 });
    bcf.bcfViewPointSetCameraDirection(viewpoint, { x: 1, y: 0, z: 0 });
    bcf.bcfViewPointSetCameraUpVector(viewpoint, { x: 0, y: 1, z: 0 });
    bcf.bcfViewPointSetAspectRatio(viewpoint, 0.45);
    bcf.bcfViewPointSetFieldOfView(viewpoint, 33);

    //
    CheckBCFProject(Module, bcf, bcfData, topicGuid, viewPointGuid);

    //
    const filePath = "..\\output\\TopicWithSnapshotExample.bcf";
    ok = bcf.bcfFileWrite(bcfData, filePath, 30);
    console.log("Write file:", ok);

    if (ok) {
        await DownloadFileFromEMS(Module, filePath);
    }
    
    ok = bcf.bcfProjectDelete(bcfData);
    console.log("Cleanup:", ok);

    console.log("Exit Topic With Snapshot Example");

    //
    //Test read
    CheckBCFFile(Module, bcf, filePath, topicGuid, viewPointGuid);
}

//
//
async function main() {

    await ExampleRawBCF();

    await ExampleBCFWrapper();

    await TopicWithSnapshot();
}

main().catch(console.error);
