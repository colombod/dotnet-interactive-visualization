import { Explorer } from '@msrvida/sanddance-explorer';
import '../css/tweak.css';
import '@msrvida/sanddance-explorer/dist/css/sanddance-explorer.css';
import { DataViewer } from './interface';
declare class SandDanceExplorerAdapter implements DataViewer {
    private readonly explorerComponent;
    constructor(explorerComponent: {
        explorer: Explorer;
    });
    loadData(data: Array<object>): void;
}
export declare function createSandDanceExplorer(container: HTMLDivElement): SandDanceExplorerAdapter;
export {};
