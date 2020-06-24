export interface DataViewer {
    loadData(data: any): void;
}
export declare type sandDanceType = "explorer" | "viewer";
export declare function createSandDance(container: HTMLDivElement, type: sandDanceType): DataViewer;
