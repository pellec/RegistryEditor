/// <reference path="../typings/toastr.d.ts" />
/// <reference path="../typings/knockout.d.ts" />
import ds = module('dataservice');
var dataservice = new ds.DataService();

declare var ko: KnockoutStatic;
declare var toastr: Toastr;

export class BaseKeyValueViewModel {
    keyName: string;
    name: string;
    valueKind: number;
    constructor(item: any) {    
        this.keyName = item.KeyName;
        this.name = item.Name;
        this.valueKind = item.ValueKind;
    }

    templateName() {
        return "";
    }   

    save() {
        console.dir(this);
    }
}

export class StringKeyValueViewModel extends BaseKeyValueViewModel{
    value: KnockoutObservableString;
    valueDescription: string;
    constructor(item: any) {
        super(item);
        this.value = ko.observable(item.Value);
        this.valueDescription = "Value is type string";
    }

    templateName() {
        return "stringvalue-template";
    }

    getData() {
        return { 
            keyName: this.keyName, 
            valueName: this.name, 
            value: this.value(), 
            valueKind: 1 
        };
    }
}

export class MultiStringKeyValueViewModel extends BaseKeyValueViewModel{
    values: KnockoutObservableArray;
    valueDescription: string;
    constructor(item: any) {
        super(item);
        this.values = ko.observableArray([]);
        this.valueDescription = "Value is type string array";

        this.init(item);
    }

    init(dto: any) {
        var values = ko.utils.arrayMap(dto.Value, function (v) {
            return new StringValueModel(v);
        });

        this.values(values);
    }

    templateName() {
        return "multistringvalue-template";
    }

    getData() {
        return { 
            keyName: this.keyName, 
            valueName: this.name, 
            value: ko.utils.arrayMap(this.values(), function (v) { return v.value(); }), 
            valueKind: 7
        };
    }

    addValue() {
        this.values.push(new StringValueModel());
    }
}

class StringValueModel {
    value: KnockoutObservableString;
    constructor(value?:string) {
        this.value = value ? ko.observable(value) : ko.observable('');
    }
}