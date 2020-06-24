import { createSandDanceExplorer } from "./explorer-widget";

export interface DataViewer {
    loadData(data: any): void;
}

export type sandDanceType = "explorer" | "viewer"

export function createSandDance(container: HTMLDivElement, type: sandDanceType): DataViewer {
    switch (type){
        case "explorer":
            return createSandDanceExplorer(container);
        default:
            throw new Error(`type ${type} is not supported`);
    }
}

