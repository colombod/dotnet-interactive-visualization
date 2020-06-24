import * as deck from '@deck.gl/core';
import * as layers from '@deck.gl/layers';
import * as luma from '@luma.gl/core';
import { fluentUI } from './fluentUIComponents';
import * as vega from 'vega';
import { Explorer, use, Snapshot } from '@msrvida/sanddance-explorer';
import ReactDOM from 'react-dom';

import '../css/tweak.css';
import '@msrvida/sanddance-explorer/dist/css/sanddance-explorer.css';
import { DataViewer } from './interface';
import React from 'react';

use(fluentUI, vega, deck as any, layers, luma);

class SandDanceExplorerAdapter implements DataViewer {

    constructor(private readonly explorerComponent: { explorer: Explorer }) {

    }

    loadData(data: Array<object>): void {
        this.explorerComponent.explorer.load(data);
    }

}

export function createSandDanceExplorer(container: HTMLDivElement) {

    let wrapper: { explorer: Explorer } = {
        explorer: null
    };
    let explorerProps = {
        logoClickUrl: 'https://microsoft.github.io/SandDance/',
        compactUI: true,
        key: 'explorer-key',
        mounted: (explorer: Explorer) => {
            wrapper.explorer = explorer;
            wrapper.explorer.setState({ snapshots : [] });
            wrapper.explorer.load([]);
        },

        snapshotProps: {
            getTopActions: (snapshots: Snapshot[]) => {
                const items = [
                    {
                        key: 'saveAsWidgetState',
                        text: 'Save as Widget State',
                        disabled: snapshots.length === 0,
                        onClick: () => this.saveSnapshots(snapshots),
                    },
                ];
                return items;
            }
        },
    };

    ReactDOM.render(React.createElement(Explorer, explorerProps), container);
    return new SandDanceExplorerAdapter(wrapper);
}