"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.createSandDanceExplorer = void 0;
const deck = __importStar(require("@deck.gl/core"));
const layers = __importStar(require("@deck.gl/layers"));
const luma = __importStar(require("@luma.gl/core"));
const fluentUIComponents_1 = require("./fluentUIComponents");
const vega = __importStar(require("vega"));
const sanddance_explorer_1 = require("@msrvida/sanddance-explorer");
const react_dom_1 = __importDefault(require("react-dom"));
const react_1 = __importDefault(require("react"));
require("../css/tweak.css");
require("@msrvida/sanddance-explorer/dist/css/sanddance-explorer.css");
sanddance_explorer_1.use(fluentUIComponents_1.fluentUI, vega, deck, layers, luma);
class SandDanceExplorerAdapter {
    constructor(explorer) {
        this.explorer = explorer;
    }
    render(data) {
        this.explorer.load(data);
    }
}
function createSandDanceExplorer(container) {
    let explorer = react_1.default.createElement(sanddance_explorer_1.Explorer);
    react_dom_1.default.render(explorer, container);
    return new SandDanceExplorerAdapter(explorer);
}
exports.createSandDanceExplorer = createSandDanceExplorer;
//# sourceMappingURL=explorer-widget.js.map