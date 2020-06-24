"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.fluentUI = void 0;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
const Button_1 = require("@fluentui/react/lib/Button");
const ChoiceGroup_1 = require("@fluentui/react/lib/ChoiceGroup");
const ComboBox_1 = require("@fluentui/react/lib/ComboBox");
const CommandBar_1 = require("@fluentui/react/lib/CommandBar");
const ContextualMenu_1 = require("@fluentui/react/lib/ContextualMenu");
const Utilities_1 = require("@fluentui/react/lib/Utilities");
const Dialog_1 = require("@fluentui/react/lib/Dialog");
const Dropdown_1 = require("@fluentui/react/lib/Dropdown");
const Styling_1 = require("@fluentui/react/lib/Styling");
const Icon_1 = require("@fluentui/react/lib/Icon");
const Icons_1 = require("@fluentui/react/lib/Icons");
const Label_1 = require("@fluentui/react/lib/Label");
const Modal_1 = require("@fluentui/react/lib/Modal");
const Slider_1 = require("@fluentui/react/lib/Slider");
const Spinner_1 = require("@fluentui/react/lib/Spinner");
const TextField_1 = require("@fluentui/react/lib/TextField");
const Toggle_1 = require("@fluentui/react/lib/Toggle");
Icons_1.initializeIcons();
exports.fluentUI = {
    ActionButton: Button_1.ActionButton,
    ChoiceGroup: ChoiceGroup_1.ChoiceGroup,
    ComboBox: ComboBox_1.ComboBox,
    CommandBar: CommandBar_1.CommandBar,
    ContextualMenuItemType: ContextualMenu_1.ContextualMenuItemType,
    Customizer: Utilities_1.Customizer,
    DefaultButton: Button_1.DefaultButton,
    Dialog: Dialog_1.Dialog,
    DialogFooter: Dialog_1.DialogFooter,
    DialogType: Dialog_1.DialogType,
    Dropdown: Dropdown_1.Dropdown,
    DropdownMenuItemType: Dropdown_1.DropdownMenuItemType,
    Icon: Icon_1.Icon,
    IconButton: Button_1.IconButton,
    getFocusStyle: Styling_1.getFocusStyle,
    getTheme: Styling_1.getTheme,
    Label: Label_1.Label,
    loadTheme: Styling_1.loadTheme,
    Modal: Modal_1.Modal,
    PrimaryButton: Button_1.PrimaryButton,
    Slider: Slider_1.Slider,
    Spinner: Spinner_1.Spinner,
    SpinnerSize: Spinner_1.SpinnerSize,
    TextField: TextField_1.TextField,
    Toggle: Toggle_1.Toggle
};
//# sourceMappingURL=fluentUIComponents.js.map