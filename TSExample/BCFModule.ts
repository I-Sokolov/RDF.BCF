


//This is raw BCF API.
//See ExampleRawAPI in app.ts how to use it.
//See BCFModuleWrapper.ts for convinuent API with string conversions.

interface BCFModule extends EmscriptenModule {
    // Project functions
    _bcfProjectCreate(projectIdPtr?: number): number;
    _bcfProjectDelete(projectPtr: number): boolean;
    _bcfProjectIsModified(projectPtr: number): boolean;
    _bcfGetErrors(projectPtr: number, cleanLog: boolean): number;

    // File functions
    _bcfFileRead(projectPtr: number, bcfFilePathPtr: number, autofix: boolean): boolean;
    _bcfFileWrite(projectPtr: number, bcfFilePathPtr: number, version: number): boolean;

    // Options
    _bcfSetOptions(projectPtr: number, userPtr: number, autoExtent: boolean, validateIfcGuids: boolean): boolean;

    // Project info
    _bcfProjectIdGet(projectPtr: number): number;
    _bcfProjectNameGet(projectPtr: number): number;
    _bcfProjectNameSet(projectPtr: number, namePtr: number): boolean;

    // Enumeration
    _bcfEnumerationElementGet(projectPtr: number, enumeration: number, ind: number): number;
    _bcfEnumerationElementAdd(projectPtr: number, enumeration: number, elementPtr: number): boolean;
    _bcfEnumerationElementRemove(projectPtr: number, enumeration: number, elementPtr: number): boolean;

    // Topic functions
    _bcfTopicGetAt(projectPtr: number, ind: number): number;
    _bcfTopicAdd(projectPtr: number, typePtr: number, titlePtr: number, statusPtr: number, guidPtr: number): number;
    _bcfTopicRemove(topicPtr: number): boolean;

    _bcfTopicGetGuid(topicPtr: number): number;
    _bcfTopicGetServerAssignedId(topicPtr: number): number;
    _bcfTopicGetTopicStatus(topicPtr: number): number;
    _bcfTopicGetTopicType(topicPtr: number): number;
    _bcfTopicGetTitle(topicPtr: number): number;
    _bcfTopicGetPriority(topicPtr: number): number;
    _bcfTopicGetCreationDate(topicPtr: number): number;
    _bcfTopicGetCreationAuthor(topicPtr: number): number;
    _bcfTopicGetModifiedDate(topicPtr: number): number;
    _bcfTopicGetModifiedAuthor(topicPtr: number): number;
    _bcfTopicGetDueDate(topicPtr: number): number;
    _bcfTopicGetAssignedTo(topicPtr: number): number;
    _bcfTopicGetDescription(topicPtr: number): number;
    _bcfTopicGetStage(topicPtr: number): number;
    _bcfTopicGetIndex(topicPtr: number): number;

    _bcfTopicSetServerAssignedId(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetTopicStatus(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetTopicType(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetTitle(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetPriority(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetDueDate(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetAssignedTo(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetDescription(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetStage(topicPtr: number, valPtr: number): boolean;
    _bcfTopicSetIndex(topicPtr: number, val: number): boolean;

    // BIM file functions
    _bcfBimFileGetAt(topicPtr: number, ind: number): number;
    _bcfBimFileAdd(topicPtr: number, filePathPtr: number, isExternal: boolean): number;
    _bcfBimFileRemove(filePtr: number): boolean;

    _bcfBimFileGetIsExternal(filePtr: number): boolean;
    _bcfBimFileGetFilename(filePtr: number): number;
    _bcfBimFileGetDate(filePtr: number): number;
    _bcfBimFileGetReference(filePtr: number): number;
    _bcfBimFileGetIfcProject(filePtr: number): number;
    _bcfBimFileGetIfcSpatialStructureElement(filePtr: number): number;

    _bcfBimFileSetIsExternal(filePtr: number, val: boolean): boolean;
    _bcfBimFileSetFilename(filePtr: number, valPtr: number): boolean;
    _bcfBimFileSetDate(filePtr: number, valPtr: number): boolean;
    _bcfBimFileSetReference(filePtr: number, valPtr: number): boolean;
    _bcfBimFileSetIfcProject(filePtr: number, valPtr: number): boolean;
    _bcfBimFileSetIfcSpatialStructureElement(filePtr: number, valPtr: number): boolean;

    // ViewPoint functions
    _bcfViewPointGetAt(topicPtr: number, ind: number): number;
    _bcfViewPointAdd(topicPtr: number, guidPtr: number): number;
    _bcfViewPointRemove(viewPointPtr: number): boolean;

    _bcfViewPointGetGuid(viewPointPtr: number): number;
    _bcfViewPointGetSnapshot(viewPointPtr: number): number;
    _bcfViewPointGetDefaultVisibility(viewPointPtr: number): boolean;
    _bcfViewPointGetSpaceVisible(viewPointPtr: number): boolean;
    _bcfViewPointGetSpaceBoundariesVisible(viewPointPtr: number): boolean;
    _bcfViewPointGetOpeningsVisible(viewPointPtr: number): boolean;
    _bcfViewPointGetCameraType(viewPointPtr: number): number;
    _bcfViewPointGetCameraViewPoint(viewPointPtr: number, retPtPtr: number): boolean;
    _bcfViewPointGetCameraDirection(viewPointPtr: number, retPtPtr: number): boolean;
    _bcfViewPointGetCameraUpVector(viewPointPtr: number, retPtPtr: number): boolean;
    _bcfViewPointGetViewToWorldScale(viewPointPtr: number): number;
    _bcfViewPointGetFieldOfView(viewPointPtr: number): number;
    _bcfViewPointGetAspectRatio(viewPointPtr: number): number;

    _bcfViewPointSetSnapshot(viewPointPtr: number, filePathPtr: number): boolean;
    _bcfViewPointSetDefaultVisibility(viewPointPtr: number, val: boolean): boolean;
    _bcfViewPointSetSpaceVisible(viewPointPtr: number, val: boolean): boolean;
    _bcfViewPointSetSpaceBoundariesVisible(viewPointPtr: number, val: boolean): boolean;
    _bcfViewPointSetOpeningsVisible(viewPointPtr: number, val: boolean): boolean;
    _bcfViewPointSetCameraType(viewPointPtr: number, val: number): boolean;
    _bcfViewPointSetCameraViewPoint(viewPointPtr: number, ptPtr: number): boolean;
    _bcfViewPointSetCameraDirection(viewPointPtr: number, ptPtr: number): boolean;
    _bcfViewPointSetCameraUpVector(viewPointPtr: number, ptPtr: number): boolean;
    _bcfViewPointSetViewToWorldScale(viewPointPtr: number, val: number): boolean;
    _bcfViewPointSetFieldOfView(viewPointPtr: number, val: number): boolean;
    _bcfViewPointSetAspectRatio(viewPointPtr: number, val: number): boolean;

    // Comment functions
    _bcfCommentGetAt(topicPtr: number, ind: number): number;
    _bcfCommentAdd(topicPtr: number, guidPtr: number): number;
    _bcfCommentRemove(commentPtr: number): boolean;

    _bcfCommentGetGuid(commentPtr: number): number;
    _bcfCommentGetDate(commentPtr: number): number;
    _bcfCommentGetAuthor(commentPtr: number): number;
    _bcfCommentGetModifiedDate(commentPtr: number): number;
    _bcfCommentGetModifiedAuthor(commentPtr: number): number;
    _bcfCommentGetText(commentPtr: number): number;
    _bcfCommentGetViewPoint(commentPtr: number): number;

    _bcfCommentSetText(commentPtr: number, textPtr: number): boolean;
    _bcfCommentSetViewPoint(commentPtr: number, viewPointPtr: number): boolean;

    // Document reference functions
    _bcfDocumentReferenceGetAt(topicPtr: number, ind: number): number;
    _bcfDocumentReferenceAdd(topicPtr: number, filePathPtr: number, isExternal: boolean, guidPtr: number): number;
    _bcfDocumentReferenceRemove(documentRefPtr: number): boolean;

    _bcfDocumentReferenceGetGuid(documentRefPtr: number): number;
    _bcfDocumentReferenceGetFilePath(documentRefPtr: number): number;
    _bcfDocumentReferenceGetIsExternal(documentRefPtr: number): boolean;
    _bcfDocumentReferenceGetDescription(documentRefPtr: number): number;

    _bcfDocumentReferenceSetFilePath(documentRefPtr: number, filePathPtr: number, isExternal: boolean): boolean;
    _bcfDocumentReferenceSetDescription(documentRefPtr: number, valuePtr: number): boolean;

    //selection
    _bcfViewPointSelectionAdd(viewPoint: number, guidPtr: number): number;
    _bcfViewPointSelectionGetAt(viewPoint: number, ind: number) : number;

    //exceptions
    _bcfViewPointExceptionAdd(viewPoint: number, guidPtr: number): number;
    _bcfViewPointExceptionGetAt(viewPoint: number, ind: number): number;

    // Component functions
    _bcfViewComponentRemove(componentPtr: number): boolean;
    _bcfComponentGetIfcGuid(componentPtr: number): number;
    _bcfComponentGetOriginatingSystem(componentPtr: number): number;
    _bcfComponentGetAuthoringToolId(componentPtr: number): number;
    _bcfComponentSetIfcGuid(componentPtr: number, valPtr: number): boolean;
    _bcfComponentSetOriginatingSystem(componentPtr: number, valPtr: number): boolean;
    _bcfComponentSetAuthoringToolId(componentPtr: number, valPtr: number): boolean;

    // Coloring functions
    _bcfColoringAdd(viewPointPtr: number, colorPtr: number): number;
    _bcfColoringGetAt(viewPointPtr: number, ind: number): number;
    _bcfColoringRemove(colorPtr: number): boolean;
    _bcfColoringGetColor(colorPtr: number): number;
    _bcfColoringSetColor(colorPtr: number, colorPtrVal: number): boolean;
    _bcfColoringComponentAdd(colorPtr: number, ifcGuidPtr: number): number;
    _bcfColoringComponentGetAt(colorPtr: number, ind: number): number;

    // Line functions
    _bcfLineAdd(viewPointPtr: number, startPtr: number, endPtr: number): number;
    _bcfLineGetAt(viewPointPtr: number, ind: number): number;
    _bcfLineRemove(linePtr: number): boolean;
    _bcfLineGetStartPoint(linePtr: number, retPtPtr: number): boolean;
    _bcfLineGetEndPoint(linePtr: number, retPtPtr: number): boolean;
    _bcfLineSetStartPoint(linePtr: number, ptPtr: number): boolean;
    _bcfLineSetEndPoint(linePtr: number, ptPtr: number): boolean;

    // Clipping plane functions
    _bcfClippingPlaneAdd(viewPointPtr: number, locationPtr: number, directionPtr: number): number;
    _bcfClippingPlaneGetAt(viewPointPtr: number, ind: number): number;
    _bcfClippingPlaneRemove(clippingPlanePtr: number): boolean;
    _bcfClippingPlaneGetLocation(clippingPlanePtr: number, retPtPtr: number): boolean;
    _bcfClippingPlaneGetDirection(clippingPlanePtr: number, retPtPtr: number): boolean;
    _bcfClippingPlaneSetLocation(clippingPlanePtr: number, ptPtr: number): boolean;
    _bcfClippingPlaneSetDirection(clippingPlanePtr: number, ptPtr: number): boolean;

    // Bitmap functions
    _bcfBitmapAdd(viewPointPtr: number, filePathPtr: number, format: number, locationPtr: number, normalPtr: number, upPtr: number, height: number): number;
    _bcfBitmapGetAt(viewPointPtr: number, ind: number): number;
    _bcfBitmapRemove(bitmapPtr: number): boolean;
    _bcfBitmapGetFormat(bitmapPtr: number): number;
    _bcfBitmapGetReference(bitmapPtr: number): number;
    _bcfBitmapGetLocation(bitmapPtr: number, retPtPtr: number): boolean;
    _bcfBitmapGetNormal(bitmapPtr: number, retPtPtr: number): boolean;
    _bcfBitmapGetUp(bitmapPtr: number, retPtPtr: number): boolean;
    _bcfBitmapGetHeight(bitmapPtr: number): number;
    _bcfBitmapSetFormat(bitmapPtr: number, val: number): boolean;
    _bcfBitmapSetReference(bitmapPtr: number, valPtr: number): boolean;
    _bcfBitmapSetLocation(bitmapPtr: number, ptPtr: number): boolean;
    _bcfBitmapSetNormal(bitmapPtr: number, ptPtr: number): boolean;
    _bcfBitmapSetUp(bitmapPtr: number, ptPtr: number): boolean;
    _bcfBitmapSetHeight(bitmapPtr: number, val: number): boolean;

    // BimSnippet functions
    _bcfTopicGetBimSnippet(topicPtr: number, forceCreate: boolean): number;
    _bcfBimSnippetRemove(snippetPtr: number): boolean;
    _bcfBimSnippetGetSnippetType(snippetPtr: number): number;
    _bcfBimSnippetGetIsExternal(snippetPtr: number): boolean;
    _bcfBimSnippetGetReference(snippetPtr: number): number;
    _bcfBimSnippetGetReferenceSchema(snippetPtr: number): number;
    _bcfBimSnippetSetSnippetType(snippetPtr: number, valPtr: number): boolean;
    _bcfBimSnippetSetIsExternal(snippetPtr: number, val: boolean): boolean;
    _bcfBimSnippetSetReference(snippetPtr: number, valPtr: number): boolean;
    _bcfBimSnippetSetReferenceSchema(snippetPtr: number, valPtr: number): boolean;

    // Reference links
    _bcfReferenceLinkAdd(topicPtr: number, valPtr: number): boolean;
    _bcfReferenceLinkGetAt(topicPtr: number, ind: number): number;
    _bcfReferenceLinkRemove(topicPtr: number, valPtr: number): boolean;

    // Labels
    _bcfLabelAdd(topicPtr: number, valPtr: number): boolean;
    _bcfLabelGetAt(topicPtr: number, ind: number): number;
    _bcfLabelRemove(topicPtr: number, valPtr: number): boolean;

    // Related topics
    _bcfRelatedTopicAdd(topicPtr: number, relatedPtr: number): boolean;
    _bcfRelatedTopicGetAt(topicPtr: number, ind: number): number;
    _bcfRelatedTopicRemove(topicPtr: number, relatedPtr: number): boolean;

    // Memory and string helpers
    _malloc(size: number): number;
    _free(ptr: number): void;
    UTF8ToString(ptr: number): string;
    stringToUTF8(str: string, outPtr: number, maxBytesToWrite: number): void;

    FS: typeof FS;
}

