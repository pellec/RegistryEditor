/// <reference path="../typings/knockout.d.ts" />
declare var ko: KnockoutStatic;

import vm = module('viewModel');

export class Bootstrapper {
    run() {
        ko.applyBindings(new vm.ViewModel());
    }
}