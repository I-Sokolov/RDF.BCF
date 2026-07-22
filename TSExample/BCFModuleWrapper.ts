type CStringFunc = (...args: number[]) => number;

export interface BCFPoint {
    x: number;
    y: number;
    z: number;
}

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
        const len = (str.length + 1) * 4; // max for UTF-8
        const ptr = this.module._malloc(len);
        this.module.stringToUTF8(str, ptr, len);
        return ptr;
    }

    private freePtr(ptr: number): void {
        if (ptr) {
            this.module._free(ptr);
        }
    }

    private withString<T>(str: string, callback: (ptr: number) => T): T {
        const ptr = this.stringToPtr(str);
        try {
            return callback(ptr);
        } finally {
            this.freePtr(ptr);
        }
    }

    /**
     * Allocate a BCFPoint in WASM memory.
     *
     * IMPORTANT:
     * This assumes BCFPoint consists of three consecutive double values:
     * x, y, z.
     */
    private pointToPtr(point: BCFPoint): number {
        const ptr = this.module._malloc(24);

        const heap = this.module.HEAPF64;
        const index = ptr / 8;

        heap[index] = point.x;
        heap[index + 1] = point.y;
        heap[index + 2] = point.z;

        return ptr;
    }

    private ptrToPoint(ptr: number): BCFPoint {
        const heap = this.module.HEAPF64;
        const index = ptr / 8;

        return {
            x: heap[index],
            y: heap[index + 1],
            z: heap[index + 2]
        };
    }

    private withPoint<T>(point: BCFPoint, callback: (ptr: number) => T): T {
        const ptr = this.pointToPtr(point);
        try {
            return callback(ptr);
        } finally {
            this.freePtr(ptr);
        }
    }

    private getPoint(
        callback: (ptr: number) => boolean
    ): BCFPoint | undefined {
        const ptr = this.module._malloc(24);

        try {
            const result = callback(ptr);

            if (!result) {
                return undefined;
            }

            return this.ptrToPoint(ptr);
        } finally {
            this.freePtr(ptr);
        }
    }

    // ============================================================
    // Project
    // ============================================================

    bcfProjectCreate(projectId?: string): number {
        if (projectId !== undefined) {
            return this.withString(projectId, ptr =>
                this.module._bcfProjectCreate(ptr)
            );
        }

        return this.module._bcfProjectCreate(0);
    }

    bcfProjectDelete(projectPtr: number): boolean {
        return this.module._bcfProjectDelete(projectPtr);
    }

    bcfProjectIsModified(projectPtr: number): boolean {
        return this.module._bcfProjectIsModified(projectPtr);
    }

    bcfGetErrors(
        projectPtr: number,
        cleanLog: boolean = false
    ): string {
        const ptr = this.module._bcfGetErrors(projectPtr, cleanLog);
        return this.ptrToString(ptr);
    }

    bcfFileRead(
        projectPtr: number,
        bcfFilePath: string,
        autofix: boolean = false
    ): boolean {
        return this.withString(bcfFilePath, ptr =>
            this.module._bcfFileRead(projectPtr, ptr, autofix)
        );
    }

    bcfFileWrite(
        projectPtr: number,
        bcfFilePath: string,
        version: number
    ): boolean {
        return this.withString(bcfFilePath, ptr =>
            this.module._bcfFileWrite(projectPtr, ptr, version)
        );
    }

    bcfSetOptions(
        projectPtr: number,
        user: string,
        autoExtent: boolean,
        validateIfcGuids: boolean
    ): boolean {
        return this.withString(user, ptr =>
            this.module._bcfSetOptions(
                projectPtr,
                ptr,
                autoExtent,
                validateIfcGuids
            )
        );
    }

    bcfProjectIdGet(projectPtr: number): string {
        return this.ptrToString(
            this.module._bcfProjectIdGet(projectPtr)
        );
    }

    bcfProjectNameGet(projectPtr: number): string {
        return this.ptrToString(
            this.module._bcfProjectNameGet(projectPtr)
        );
    }

    bcfProjectNameSet(
        projectPtr: number,
        name: string
    ): boolean {
        return this.withString(name, ptr =>
            this.module._bcfProjectNameSet(projectPtr, ptr)
        );
    }

    // ============================================================
    // Enumeration
    // ============================================================

    bcfEnumerationElementGet(
        projectPtr: number,
        enumeration: number,
        ind: number
    ): string {
        return this.ptrToString(
            this.module._bcfEnumerationElementGet(
                projectPtr,
                enumeration,
                ind
            )
        );
    }

    bcfEnumerationElementAdd(
        projectPtr: number,
        enumeration: number,
        element: string
    ): boolean {
        return this.withString(element, ptr =>
            this.module._bcfEnumerationElementAdd(
                projectPtr,
                enumeration,
                ptr
            )
        );
    }

    bcfEnumerationElementRemove(
        projectPtr: number,
        enumeration: number,
        element: string
    ): boolean {
        return this.withString(element, ptr =>
            this.module._bcfEnumerationElementRemove(
                projectPtr,
                enumeration,
                ptr
            )
        );
    }

    // ============================================================
    // Topic
    // ============================================================

    bcfTopicGetAt(
        projectPtr: number,
        ind: number
    ): number {
        return this.module._bcfTopicGetAt(projectPtr, ind);
    }

    bcfTopicAdd(
        projectPtr: number,
        type: string,
        title: string,
        status: string,
        guid: string
    ): number {
        return this.withString(type, typePtr =>
            this.withString(title, titlePtr =>
                this.withString(status, statusPtr =>
                    this.withString(guid, guidPtr =>
                        this.module._bcfTopicAdd(
                            projectPtr,
                            typePtr,
                            titlePtr,
                            statusPtr,
                            guidPtr
                        )
                    )
                )
            )
        );
    }

    bcfTopicRemove(topicPtr: number): boolean {
        return this.module._bcfTopicRemove(topicPtr);
    }

    bcfTopicGetGuid(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetGuid(topicPtr)
        );
    }

    bcfTopicGetServerAssignedId(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetServerAssignedId(topicPtr)
        );
    }

    bcfTopicGetTopicStatus(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetTopicStatus(topicPtr)
        );
    }

    bcfTopicGetTopicType(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetTopicType(topicPtr)
        );
    }

    bcfTopicGetTitle(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetTitle(topicPtr)
        );
    }

    bcfTopicGetPriority(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetPriority(topicPtr)
        );
    }

    bcfTopicGetCreationDate(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetCreationDate(topicPtr)
        );
    }

    bcfTopicGetCreationAuthor(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetCreationAuthor(topicPtr)
        );
    }

    bcfTopicGetModifiedDate(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetModifiedDate(topicPtr)
        );
    }

    bcfTopicGetModifiedAuthor(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetModifiedAuthor(topicPtr)
        );
    }

    bcfTopicGetDueDate(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetDueDate(topicPtr)
        );
    }

    bcfTopicGetAssignedTo(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetAssignedTo(topicPtr)
        );
    }

    bcfTopicGetDescription(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetDescription(topicPtr)
        );
    }

    bcfTopicGetStage(topicPtr: number): string {
        return this.ptrToString(
            this.module._bcfTopicGetStage(topicPtr)
        );
    }

    bcfTopicGetIndex(topicPtr: number): number {
        return this.module._bcfTopicGetIndex(topicPtr);
    }

    bcfTopicSetServerAssignedId(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetServerAssignedId(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetTopicStatus(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetTopicStatus(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetTopicType(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetTopicType(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetTitle(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetTitle(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetPriority(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetPriority(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetDueDate(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetDueDate(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetAssignedTo(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetAssignedTo(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetDescription(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetDescription(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetStage(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfTopicSetStage(
                topicPtr,
                ptr
            )
        );
    }

    bcfTopicSetIndex(
        topicPtr: number,
        val: number
    ): boolean {
        return this.module._bcfTopicSetIndex(
            topicPtr,
            val
        );
    }

    // ============================================================
    // BIM File
    // ============================================================

    bcfBimFileGetAt(
        topicPtr: number,
        ind: number
    ): number {
        return this.module._bcfBimFileGetAt(
            topicPtr,
            ind
        );
    }

    bcfBimFileAdd(
        topicPtr: number,
        filePath: string,
        isExternal: boolean
    ): number {
        return this.withString(filePath, ptr =>
            this.module._bcfBimFileAdd(
                topicPtr,
                ptr,
                isExternal
            )
        );
    }

    bcfBimFileRemove(filePtr: number): boolean {
        return this.module._bcfBimFileRemove(filePtr);
    }

    bcfBimFileGetIsExternal(filePtr: number): boolean {
        return this.module._bcfBimFileGetIsExternal(filePtr);
    }

    bcfBimFileGetFilename(filePtr: number): string {
        return this.ptrToString(
            this.module._bcfBimFileGetFilename(filePtr)
        );
    }

    bcfBimFileGetDate(filePtr: number): string {
        return this.ptrToString(
            this.module._bcfBimFileGetDate(filePtr)
        );
    }

    bcfBimFileGetReference(filePtr: number): string {
        return this.ptrToString(
            this.module._bcfBimFileGetReference(filePtr)
        );
    }

    bcfBimFileGetIfcProject(filePtr: number): string {
        return this.ptrToString(
            this.module._bcfBimFileGetIfcProject(filePtr)
        );
    }

    bcfBimFileGetIfcSpatialStructureElement(
        filePtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfBimFileGetIfcSpatialStructureElement(
                filePtr
            )
        );
    }

    bcfBimFileSetIsExternal(
        filePtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfBimFileSetIsExternal(
            filePtr,
            val
        );
    }

    bcfBimFileSetFilename(
        filePtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimFileSetFilename(
                filePtr,
                ptr
            )
        );
    }

    bcfBimFileSetDate(
        filePtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimFileSetDate(
                filePtr,
                ptr
            )
        );
    }

    bcfBimFileSetReference(
        filePtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimFileSetReference(
                filePtr,
                ptr
            )
        );
    }

    bcfBimFileSetIfcProject(
        filePtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimFileSetIfcProject(
                filePtr,
                ptr
            )
        );
    }

    bcfBimFileSetIfcSpatialStructureElement(
        filePtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimFileSetIfcSpatialStructureElement(
                filePtr,
                ptr
            )
        );
    }

    // ============================================================
    // ViewPoint
    // ============================================================

    bcfViewPointGetAt(
        topicPtr: number,
        ind: number
    ): number {
        return this.module._bcfViewPointGetAt(
            topicPtr,
            ind
        );
    }

    bcfViewPointAdd(
        topicPtr: number,
        guid: string
    ): number {
        return this.withString(guid, ptr =>
            this.module._bcfViewPointAdd(
                topicPtr,
                ptr
            )
        );
    }

    bcfViewPointRemove(viewPointPtr: number): boolean {
        return this.module._bcfViewPointRemove(
            viewPointPtr
        );
    }

    bcfViewPointGetGuid(viewPointPtr: number): string {
        return this.ptrToString(
            this.module._bcfViewPointGetGuid(
                viewPointPtr
            )
        );
    }

    bcfViewPointGetSnapshot(viewPointPtr: number): string {
        return this.ptrToString(
            this.module._bcfViewPointGetSnapshot(
                viewPointPtr
            )
        );
    }

    bcfViewPointGetDefaultVisibility(
        viewPointPtr: number
    ): boolean {
        return this.module._bcfViewPointGetDefaultVisibility(
            viewPointPtr
        );
    }

    bcfViewPointGetSpaceVisible(
        viewPointPtr: number
    ): boolean {
        return this.module._bcfViewPointGetSpaceVisible(
            viewPointPtr
        );
    }

    bcfViewPointGetSpaceBoundariesVisible(
        viewPointPtr: number
    ): boolean {
        return this.module._bcfViewPointGetSpaceBoundariesVisible(
            viewPointPtr
        );
    }

    bcfViewPointGetOpeningsVisible(
        viewPointPtr: number
    ): boolean {
        return this.module._bcfViewPointGetOpeningsVisible(
            viewPointPtr
        );
    }

    bcfViewPointGetCameraType(
        viewPointPtr: number
    ): number {
        return this.module._bcfViewPointGetCameraType(
            viewPointPtr
        );
    }

    bcfViewPointGetCameraViewPoint(
        viewPointPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfViewPointGetCameraViewPoint(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointGetCameraDirection(
        viewPointPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfViewPointGetCameraDirection(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointGetCameraUpVector(
        viewPointPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfViewPointGetCameraUpVector(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointGetViewToWorldScale(
        viewPointPtr: number
    ): number {
        return this.module._bcfViewPointGetViewToWorldScale(
            viewPointPtr
        );
    }

    bcfViewPointGetFieldOfView(
        viewPointPtr: number
    ): number {
        return this.module._bcfViewPointGetFieldOfView(
            viewPointPtr
        );
    }

    bcfViewPointGetAspectRatio(
        viewPointPtr: number
    ): number {
        return this.module._bcfViewPointGetAspectRatio(
            viewPointPtr
        );
    }

    bcfViewPointSetSnapshot(
        viewPointPtr: number,
        filePath: string
    ): boolean {
        return this.withString(filePath, ptr =>
            this.module._bcfViewPointSetSnapshot(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointSetDefaultVisibility(
        viewPointPtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfViewPointSetDefaultVisibility(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetSpaceVisible(
        viewPointPtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfViewPointSetSpaceVisible(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetSpaceBoundariesVisible(
        viewPointPtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfViewPointSetSpaceBoundariesVisible(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetOpeningsVisible(
        viewPointPtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfViewPointSetOpeningsVisible(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetCameraType(
        viewPointPtr: number,
        val: number
    ): boolean {
        return this.module._bcfViewPointSetCameraType(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetCameraViewPoint(
        viewPointPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfViewPointSetCameraViewPoint(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointSetCameraDirection(
        viewPointPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfViewPointSetCameraDirection(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointSetCameraUpVector(
        viewPointPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfViewPointSetCameraUpVector(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointSetViewToWorldScale(
        viewPointPtr: number,
        val: number
    ): boolean {
        return this.module._bcfViewPointSetViewToWorldScale(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetFieldOfView(
        viewPointPtr: number,
        val: number
    ): boolean {
        return this.module._bcfViewPointSetFieldOfView(
            viewPointPtr,
            val
        );
    }

    bcfViewPointSetAspectRatio(
        viewPointPtr: number,
        val: number
    ): boolean {
        return this.module._bcfViewPointSetAspectRatio(
            viewPointPtr,
            val
        );
    }

    // ============================================================
    // Comment
    // ============================================================

    bcfCommentGetAt(
        topicPtr: number,
        ind: number
    ): number {
        return this.module._bcfCommentGetAt(
            topicPtr,
            ind
        );
    }

    bcfCommentAdd(
        topicPtr: number,
        guid: string
    ): number {
        return this.withString(guid, ptr =>
            this.module._bcfCommentAdd(
                topicPtr,
                ptr
            )
        );
    }

    bcfCommentRemove(commentPtr: number): boolean {
        return this.module._bcfCommentRemove(
            commentPtr
        );
    }

    bcfCommentGetGuid(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetGuid(commentPtr)
        );
    }

    bcfCommentGetDate(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetDate(commentPtr)
        );
    }

    bcfCommentGetAuthor(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetAuthor(commentPtr)
        );
    }

    bcfCommentGetModifiedDate(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetModifiedDate(commentPtr)
        );
    }

    bcfCommentGetModifiedAuthor(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetModifiedAuthor(commentPtr)
        );
    }

    bcfCommentGetText(commentPtr: number): string {
        return this.ptrToString(
            this.module._bcfCommentGetText(commentPtr)
        );
    }

    bcfCommentGetViewPoint(commentPtr: number): number {
        return this.module._bcfCommentGetViewPoint(
            commentPtr
        );
    }

    bcfCommentSetText(
        commentPtr: number,
        text: string
    ): boolean {
        return this.withString(text, ptr =>
            this.module._bcfCommentSetText(
                commentPtr,
                ptr
            )
        );
    }

    bcfCommentSetViewPoint(
        commentPtr: number,
        viewPointPtr: number
    ): boolean {
        return this.module._bcfCommentSetViewPoint(
            commentPtr,
            viewPointPtr
        );
    }

    // ============================================================
    // Document Reference
    // ============================================================

    bcfDocumentReferenceGetAt(
        topicPtr: number,
        ind: number
    ): number {
        return this.module._bcfDocumentReferenceGetAt(
            topicPtr,
            ind
        );
    }

    bcfDocumentReferenceAdd(
        topicPtr: number,
        filePath: string,
        isExternal: boolean,
        guid: string
    ): number {
        return this.withString(filePath, filePathPtr =>
            this.withString(guid, guidPtr =>
                this.module._bcfDocumentReferenceAdd(
                    topicPtr,
                    filePathPtr,
                    isExternal,
                    guidPtr
                )
            )
        );
    }

    bcfDocumentReferenceRemove(
        documentReferencePtr: number
    ): boolean {
        return this.module._bcfDocumentReferenceRemove(
            documentReferencePtr
        );
    }

    bcfDocumentReferenceGetGuid(
        documentReferencePtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfDocumentReferenceGetGuid(
                documentReferencePtr
            )
        );
    }

    bcfDocumentReferenceGetFilePath(
        documentReferencePtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfDocumentReferenceGetFilePath(
                documentReferencePtr
            )
        );
    }

    bcfDocumentReferenceGetIsExternal(
        documentReferencePtr: number
    ): boolean {
        return this.module._bcfDocumentReferenceGetIsExternal(
            documentReferencePtr
        );
    }

    bcfDocumentReferenceGetDescription(
        documentReferencePtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfDocumentReferenceGetDescription(
                documentReferencePtr
            )
        );
    }

    bcfDocumentReferenceSetFilePath(
        documentReferencePtr: number,
        filePath: string,
        isExternal: boolean
    ): boolean {
        return this.withString(filePath, ptr =>
            this.module._bcfDocumentReferenceSetFilePath(
                documentReferencePtr,
                ptr,
                isExternal
            )
        );
    }

    bcfDocumentReferenceSetDescription(
        documentReferencePtr: number,
        value: string
    ): boolean {
        return this.withString(value, ptr =>
            this.module._bcfDocumentReferenceSetDescription(
                documentReferencePtr,
                ptr
            )
        );
    }

    // ============================================================
    // Components
    // ============================================================

    bcfViewPointSelectionAdd(
        viewPointPtr: number,
        ifcGuid: string
    ): number {
        return this.withString(ifcGuid, ptr =>
            this.module._bcfViewPointSelectionAdd(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointSelectionGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfViewPointSelectionGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfViewPointExceptionAdd(
        viewPointPtr: number,
        ifcGuid: string
    ): number {
        return this.withString(ifcGuid, ptr =>
            this.module._bcfViewPointExceptionAdd(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfViewPointExceptionGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfViewPointExceptionGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfViewComponentRemove(
        componentPtr: number
    ): boolean {
        return this.module._bcfViewComponentRemove(
            componentPtr
        );
    }

    bcfComponentGetIfcGuid(componentPtr: number): string {
        return this.ptrToString(
            this.module._bcfComponentGetIfcGuid(
                componentPtr
            )
        );
    }

    bcfComponentGetOriginatingSystem(
        componentPtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfComponentGetOriginatingSystem(
                componentPtr
            )
        );
    }

    bcfComponentGetAuthoringToolId(
        componentPtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfComponentGetAuthoringToolId(
                componentPtr
            )
        );
    }

    bcfComponentSetIfcGuid(
        componentPtr: number,
        value: string
    ): boolean {
        return this.withString(value, ptr =>
            this.module._bcfComponentSetIfcGuid(
                componentPtr,
                ptr
            )
        );
    }

    bcfComponentSetOriginatingSystem(
        componentPtr: number,
        value: string
    ): boolean {
        return this.withString(value, ptr =>
            this.module._bcfComponentSetOriginatingSystem(
                componentPtr,
                ptr
            )
        );
    }

    bcfComponentSetAuthoringToolId(
        componentPtr: number,
        value: string
    ): boolean {
        return this.withString(value, ptr =>
            this.module._bcfComponentSetAuthoringToolId(
                componentPtr,
                ptr
            )
        );
    }

    // ============================================================
    // Coloring
    // ============================================================

    bcfColoringAdd(
        viewPointPtr: number,
        color: string
    ): number {
        return this.withString(color, ptr =>
            this.module._bcfColoringAdd(
                viewPointPtr,
                ptr
            )
        );
    }

    bcfColoringGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfColoringGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfColoringRemove(coloringPtr: number): boolean {
        return this.module._bcfColoringRemove(
            coloringPtr
        );
    }

    bcfColoringGetColor(coloringPtr: number): string {
        return this.ptrToString(
            this.module._bcfColoringGetColor(
                coloringPtr
            )
        );
    }

    bcfColoringSetColor(
        coloringPtr: number,
        color: string
    ): boolean {
        return this.withString(color, ptr =>
            this.module._bcfColoringSetColor(
                coloringPtr,
                ptr
            )
        );
    }

    bcfColoringComponentAdd(
        coloringPtr: number,
        ifcGuid: string
    ): number {
        return this.withString(ifcGuid, ptr =>
            this.module._bcfColoringComponentAdd(
                coloringPtr,
                ptr
            )
        );
    }

    bcfColoringComponentGetAt(
        coloringPtr: number,
        ind: number
    ): number {
        return this.module._bcfColoringComponentGetAt(
            coloringPtr,
            ind
        );
    }

    // ============================================================
    // Line
    // ============================================================

    bcfLineAdd(
        viewPointPtr: number,
        start: BCFPoint,
        end: BCFPoint
    ): number {
        return this.withPoint(start, startPtr =>
            this.withPoint(end, endPtr =>
                this.module._bcfLineAdd(
                    viewPointPtr,
                    startPtr,
                    endPtr
                )
            )
        );
    }

    bcfLineGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfLineGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfLineRemove(linePtr: number): boolean {
        return this.module._bcfLineRemove(linePtr);
    }

    bcfLineGetStartPoint(
        linePtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfLineGetStartPoint(
                linePtr,
                ptr
            )
        );
    }

    bcfLineGetEndPoint(
        linePtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfLineGetEndPoint(
                linePtr,
                ptr
            )
        );
    }

    bcfLineSetStartPoint(
        linePtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfLineSetStartPoint(
                linePtr,
                ptr
            )
        );
    }

    bcfLineSetEndPoint(
        linePtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfLineSetEndPoint(
                linePtr,
                ptr
            )
        );
    }

    // ============================================================
    // Clipping Plane
    // ============================================================

    bcfClippingPlaneAdd(
        viewPointPtr: number,
        location: BCFPoint,
        direction: BCFPoint
    ): number {
        return this.withPoint(location, locationPtr =>
            this.withPoint(direction, directionPtr =>
                this.module._bcfClippingPlaneAdd(
                    viewPointPtr,
                    locationPtr,
                    directionPtr
                )
            )
        );
    }

    bcfClippingPlaneGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfClippingPlaneGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfClippingPlaneRemove(
        clippingPlanePtr: number
    ): boolean {
        return this.module._bcfClippingPlaneRemove(
            clippingPlanePtr
        );
    }

    bcfClippingPlaneGetLocation(
        clippingPlanePtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfClippingPlaneGetLocation(
                clippingPlanePtr,
                ptr
            )
        );
    }

    bcfClippingPlaneGetDirection(
        clippingPlanePtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfClippingPlaneGetDirection(
                clippingPlanePtr,
                ptr
            )
        );
    }

    bcfClippingPlaneSetLocation(
        clippingPlanePtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfClippingPlaneSetLocation(
                clippingPlanePtr,
                ptr
            )
        );
    }

    bcfClippingPlaneSetDirection(
        clippingPlanePtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfClippingPlaneSetDirection(
                clippingPlanePtr,
                ptr
            )
        );
    }

    // ============================================================
    // Bitmap
    // ============================================================

    bcfBitmapAdd(
        viewPointPtr: number,
        filePath: string,
        format: number,
        location: BCFPoint,
        normal: BCFPoint,
        up: BCFPoint,
        height: number
    ): number {
        return this.withString(filePath, filePathPtr =>
            this.withPoint(location, locationPtr =>
                this.withPoint(normal, normalPtr =>
                    this.withPoint(up, upPtr =>
                        this.module._bcfBitmapAdd(
                            viewPointPtr,
                            filePathPtr,
                            format,
                            locationPtr,
                            normalPtr,
                            upPtr,
                            height
                        )
                    )
                )
            )
        );
    }

    bcfBitmapGetAt(
        viewPointPtr: number,
        ind: number
    ): number {
        return this.module._bcfBitmapGetAt(
            viewPointPtr,
            ind
        );
    }

    bcfBitmapRemove(bitmapPtr: number): boolean {
        return this.module._bcfBitmapRemove(bitmapPtr);
    }

    bcfBitmapGetFormat(bitmapPtr: number): number {
        return this.module._bcfBitmapGetFormat(
            bitmapPtr
        );
    }

    bcfBitmapGetReference(bitmapPtr: number): string {
        return this.ptrToString(
            this.module._bcfBitmapGetReference(
                bitmapPtr
            )
        );
    }

    bcfBitmapGetLocation(
        bitmapPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfBitmapGetLocation(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapGetNormal(
        bitmapPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfBitmapGetNormal(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapGetUp(
        bitmapPtr: number
    ): BCFPoint | undefined {
        return this.getPoint(ptr =>
            this.module._bcfBitmapGetUp(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapGetHeight(bitmapPtr: number): number {
        return this.module._bcfBitmapGetHeight(
            bitmapPtr
        );
    }

    bcfBitmapSetFormat(
        bitmapPtr: number,
        val: number
    ): boolean {
        return this.module._bcfBitmapSetFormat(
            bitmapPtr,
            val
        );
    }

    bcfBitmapSetReference(
        bitmapPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBitmapSetReference(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapSetLocation(
        bitmapPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfBitmapSetLocation(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapSetNormal(
        bitmapPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfBitmapSetNormal(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapSetUp(
        bitmapPtr: number,
        point: BCFPoint
    ): boolean {
        return this.withPoint(point, ptr =>
            this.module._bcfBitmapSetUp(
                bitmapPtr,
                ptr
            )
        );
    }

    bcfBitmapSetHeight(
        bitmapPtr: number,
        val: number
    ): boolean {
        return this.module._bcfBitmapSetHeight(
            bitmapPtr,
            val
        );
    }

    // ============================================================
    // BIM Snippet
    // ============================================================

    bcfTopicGetBimSnippet(
        topicPtr: number,
        forceCreate: boolean
    ): number {
        return this.module._bcfTopicGetBimSnippet(
            topicPtr,
            forceCreate
        );
    }

    bcfBimSnippetRemove(
        snippetPtr: number
    ): boolean {
        return this.module._bcfBimSnippetRemove(
            snippetPtr
        );
    }

    bcfBimSnippetGetSnippetType(
        snippetPtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfBimSnippetGetSnippetType(
                snippetPtr
            )
        );
    }

    bcfBimSnippetGetIsExternal(
        snippetPtr: number
    ): boolean {
        return this.module._bcfBimSnippetGetIsExternal(
            snippetPtr
        );
    }

    bcfBimSnippetGetReference(
        snippetPtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfBimSnippetGetReference(
                snippetPtr
            )
        );
    }

    bcfBimSnippetGetReferenceSchema(
        snippetPtr: number
    ): string {
        return this.ptrToString(
            this.module._bcfBimSnippetGetReferenceSchema(
                snippetPtr
            )
        );
    }

    bcfBimSnippetSetSnippetType(
        snippetPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimSnippetSetSnippetType(
                snippetPtr,
                ptr
            )
        );
    }

    bcfBimSnippetSetIsExternal(
        snippetPtr: number,
        val: boolean
    ): boolean {
        return this.module._bcfBimSnippetSetIsExternal(
            snippetPtr,
            val
        );
    }

    bcfBimSnippetSetReference(
        snippetPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimSnippetSetReference(
                snippetPtr,
                ptr
            )
        );
    }

    bcfBimSnippetSetReferenceSchema(
        snippetPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfBimSnippetSetReferenceSchema(
                snippetPtr,
                ptr
            )
        );
    }

    // ============================================================
    // Reference Links
    // ============================================================

    bcfReferenceLinkAdd(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfReferenceLinkAdd(
                topicPtr,
                ptr
            )
        );
    }

    bcfReferenceLinkGetAt(
        topicPtr: number,
        ind: number
    ): string {
        return this.ptrToString(
            this.module._bcfReferenceLinkGetAt(
                topicPtr,
                ind
            )
        );
    }

    bcfReferenceLinkRemove(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfReferenceLinkRemove(
                topicPtr,
                ptr
            )
        );
    }

    // ============================================================
    // Labels
    // ============================================================

    bcfLabelAdd(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfLabelAdd(
                topicPtr,
                ptr
            )
        );
    }

    bcfLabelGetAt(
        topicPtr: number,
        ind: number
    ): string {
        return this.ptrToString(
            this.module._bcfLabelGetAt(
                topicPtr,
                ind
            )
        );
    }

    bcfLabelRemove(
        topicPtr: number,
        val: string
    ): boolean {
        return this.withString(val, ptr =>
            this.module._bcfLabelRemove(
                topicPtr,
                ptr
            )
        );
    }

    // ============================================================
    // Related Topics
    // ============================================================

    bcfRelatedTopicAdd(
        topicPtr: number,
        relatedTopicPtr: number
    ): boolean {
        return this.module._bcfRelatedTopicAdd(
            topicPtr,
            relatedTopicPtr
        );
    }

    bcfRelatedTopicGetAt(
        topicPtr: number,
        ind: number
    ): number {
        return this.module._bcfRelatedTopicGetAt(
            topicPtr,
            ind
        );
    }

    bcfRelatedTopicRemove(
        topicPtr: number,
        relatedTopicPtr: number
    ): boolean {
        return this.module._bcfRelatedTopicRemove(
            topicPtr,
            relatedTopicPtr
        );
    }

}
