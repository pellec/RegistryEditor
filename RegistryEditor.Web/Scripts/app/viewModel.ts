/// <reference path="../typings/knockout.d.ts" />
/// <reference path="../typings/underscore.d.ts" />
/// <reference path="../typings/sammyjs.d.ts" />
declare var ko: KnockoutStatic;
declare var $: JQueryStatic;
declare var _: UnderscoreStatic;

import ds = module('dataservice');
import kv = module('keyValueViewModel');
import kvf = module('keyValueViewModelFactory');
import sm = module('serverModel');

var dataservice = new ds.DataService();

export class ViewModel {
    keys: KnockoutObservableArray;
    servers: KnockoutObservableArray;
    values: KnockoutObservableArray;
    currentValue: KnockoutObservableAny;
    isLoading: KnockoutObservableBool;
    currentKey: KnockoutObservableString;
    fadeInContainer: KnockoutObservableBool;
    serverName: KnockoutObservableString;
    newKey: KnockoutObservableString;
    newValue: KnockoutObservableString;
    breadcrumbs: KnockoutObservableArray;

    constructor() {
        this.keys = ko.observableArray([]);
        this.servers = ko.observableArray([]);
        this.values = ko.observableArray([]);
        this.currentValue = ko.observable();
        this.isLoading = ko.observable(false);
        this.currentKey = ko.observable('');
        this.fadeInContainer = ko.observable(false);
        this.serverName = ko.observable('');
        this.newKey = ko.observable('');
        this.newValue = ko.observable('');
        this.breadcrumbs = ko.observableArray([]);

        this.configureRouting();
        this.getServers();
    }

    configureRouting() {
        var self = this;
        $.sammy(function () {
            this.get(/\#\/registry\/(.*)/, function () {
                console.log('key route triggered');
                console.dir(this);
                self.handleKeyChange(this.params['splat'].join('/'));
            });

            this.get('#/server/:name', function () {
                console.log('server route triggered with name');
                console.dir(this);
                self.currentKey('');
                self.serverName(this.params.name);
                self.breadcrumbs([]);
                self.breadcrumbs.push(this.params.name);
                self.getKey();
            });

            this.get(/\#\/server\/*\/(.*)/, function () {
                console.log('server route triggered with reg');
                console.dir(this);
                var splatSplitted = this.params['splat'][0].split('/');
                self.serverName(splatSplitted[0]);
                splatSplitted.shift();
                
                self.breadcrumbs([]);
                self.breadcrumbs.push(self.serverName());

                $.each(splatSplitted, function (i, e) {
                    if(e !== ''){
                        self.breadcrumbs.push(e);
                    }
                });

                self.handleKeyChange(splatSplitted.join('/'));
                self.getKey();
            });

            this.get('', function () {
                console.log('default route triggered');
            });
        }).run();
    }

    createKey() {
        this.isLoading(true);
        dataservice.saveKey({serverName: this.serverName(), subKey: this.currentKey(), newKey: this.newKey()})
            .then(() => {
                toastr.success('Successfully created key "' + this.newKey() + '"');
                this.getKey();
            },() => {
                toastr.error('Could not create key "' + this.newKey() + '"');
            }).always(() => {
                this.isLoading(false);
                this.newKey('');
            });
    }

    saveValue(item) {
        var data = {
            serverName: this.serverName()
        };

        $.extend(data, item.getData());
        $.when(dataservice.saveValue(data))
            .done((r) => {
                this.getKey();
                toastr.success('Successfully saved value: "' +this.currentValue().name + '" in key: "' +this.currentKey() + '"!');
        }).fail((r) => {
            console.dir(r);
            toastr.error('Could not save the value ' + this.currentValue().name + 'for key ' + this.currentKey());
        }).always(() => {
            this.currentValue('');
        });
    }

    isValid(value) {
        return value !== undefined && value !== '';
    }

    handleKeyChange(key?: string) {
        this.currentKey(key);
        this.getKey();
    }

    setCurrentValue(item) {
        if (item.IsNew) {
            item.KeyName = this.currentKey();
            item.Name = this.newValue();
            this.currentValue(new kvf.KeyValueViewModelFactory().create(item));
        } else {
            this.currentValue(item);
        }
    }

    getServers() {
        $.when(dataservice.getServers()).then((result: any) => {
            this.servers.map(result, sm.ServerModel);
        });
    }

    getKey() {
        this.isLoading(true);
        this.fadeInContainer(false);
        this.keys([]);
        this.values([]);

        var query = {
            serverName: this.serverName(),
            subKey: this.currentKey() === undefined ? "" : this.currentKey()
        };

        $.when(dataservice.getKey(query)).then((result: any) => {
            this.currentKey(result.KeyName);
            this.fadeInContainer(true);
            this.keys(result.SubKeys);
            this.values.mapWithFactory(result.Values, kvf.KeyValueViewModelFactory);
        }).always(() => {
            this.isLoading(false);
        });
    }

    getKeyUrlForBreadcrumb(keyName: string){
        var url = '#/server/';
        $.each(this.breadcrumbs(), function (i, b) {
            if(b === keyName){
                url += keyName;
                return false;
            }

            url += b + '/';
        });

        return url;
    }

    getKeyUrl(keyName: string) {
        if (this.currentKey() === undefined || this.currentKey() === "") {
            return "#/server/" + this.serverName() + "/" + keyName;
        }

        return "#/server/" + this.serverName() + "/" + this.currentKey() + "/" + keyName;
    }
}