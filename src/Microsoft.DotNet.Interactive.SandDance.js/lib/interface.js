"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.createSandDance = void 0;
const explorer_widget_1 = require("./explorer-widget");
function createSandDance(container, type) {
    switch (type) {
        case "Explorer":
            return explorer_widget_1.createSandDanceExplorer(container);
        default:
            throw new Error(`type ${type} is not supported`);
    }
}
exports.createSandDance = createSandDance;
//# sourceMappingURL=interface.js.map