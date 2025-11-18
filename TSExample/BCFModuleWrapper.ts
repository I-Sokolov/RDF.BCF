

type CStringFunc = (...args: number[]) => number;

export class BCFModuleWrapper {
    private module: BCFModule;

    constructor(module: BCFModule) {
        this.module = module;
    }

    // --- Helpers ---

    private ptrToString(ptr: number): string {
        if (!ptr) return "";
        return this.module.UTF8ToString(ptr);
    }

    private stringToPtr(str: string): number {
        const len = (str.length + 1) * 4; //max for UTF-8
        const ptr = this.module._malloc(len);
        this.module.stringToUTF8(str, ptr, len);
        return ptr;
    }

    private freePtr(ptr: number) {
        if (ptr) this.module._free(ptr);
    }

    // --- Wrapped project functions ---

    bcfProjectCreate(projectId?: string): number {
        if (projectId) {
            const ptr = this.stringToPtr(projectId);
            const projectPtr = this.module._bcfProjectCreate(ptr);
            this.freePtr(ptr);
            return projectPtr;
        } else {
            return this.module._bcfProjectCreate(0);
        }
    }

    bcfProjectDelete(projectPtr: number): boolean {
        return this.module._bcfProjectDelete(projectPtr);
    }

    bcfProjectIsModified(projectPtr: number): boolean {
        return this.module._bcfProjectIsModified(projectPtr);
    }

    bcfGetErrors(projectPtr: number, cleanLog: boolean = false): string {
        const ptr = this.module._bcfGetErrors(projectPtr, cleanLog);
        return this.ptrToString(ptr);
    }

    bcfProjectIdGet(projectPtr: number): string {
        const ptr = this.module._bcfProjectIdGet(projectPtr);
        return this.ptrToString(ptr);
    }

    bcfProjectNameGet(projectPtr: number): string {
        const ptr = this.module._bcfProjectNameGet(projectPtr);
        return this.ptrToString(ptr);
    }

    bcfProjectNameSet(projectPtr: number, name: string): boolean {
        const ptr = this.stringToPtr(name);
        const result = this.module._bcfProjectNameSet(projectPtr, ptr);
        this.freePtr(ptr);
        return result;
    }

    // --- Wrapped file functions ---

    bcfFileRead(projectPtr: number, bcfFilePath: string, autofix: boolean = false): boolean {
        const ptr = this.stringToPtr(bcfFilePath);
        const result = this.module._bcfFileRead(projectPtr, ptr, autofix);
        this.freePtr(ptr);
        return result;
    }

    bcfFileWrite(projectPtr: number, bcfFilePath: string, version: number): boolean {
        const ptr = this.stringToPtr(bcfFilePath);
        const result = this.module._bcfFileWrite(projectPtr, ptr, version);
        this.freePtr(ptr);
        return result;
    }

    // --- Add more wrappers as needed ---
    // For every function returning const char*, wrap using ptrToString()
    // For every function accepting const char*, wrap using stringToPtr()
}

