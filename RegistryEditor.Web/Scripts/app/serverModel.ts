/// <reference path="../typings/knockout.d.ts" />
declare var ko: KnockoutStatic;

export class ServerModel {
    name: KnockoutObservableString;
    url: KnockoutObservableString;

    constructor(item: any) {
        this.name = ko.observable(item.Name);
        this.url = ko.observable('#/server/' + item.Name);
    }
}