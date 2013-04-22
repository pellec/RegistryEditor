define(["require", "exports", 'dataservice', 'keyValueViewModelFactory', 'serverModel'], function(require, exports, __ds__, __kvf__, __sm__) {
    var ds = __ds__;

    
    var kvf = __kvf__;

    var sm = __sm__;

    var dataservice = new ds.DataService();
    var ViewModel = (function () {
        function ViewModel() {
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
        ViewModel.prototype.configureRouting = function () {
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
                        if(e !== '') {
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
        };
        ViewModel.prototype.createKey = function () {
            var _this = this;
            this.isLoading(true);
            dataservice.saveKey({
                serverName: this.serverName(),
                subKey: this.currentKey(),
                newKey: this.newKey()
            }).then(function () {
                toastr.success('Successfully created key "' + _this.newKey() + '"');
                _this.getKey();
            }, function () {
                toastr.error('Could not create key "' + _this.newKey() + '"');
            }).always(function () {
                _this.isLoading(false);
                _this.newKey('');
            });
        };
        ViewModel.prototype.saveValue = function (item) {
            var _this = this;
            var data = {
                serverName: this.serverName()
            };
            $.extend(data, item.getData());
            $.when(dataservice.saveValue(data)).done(function (r) {
                _this.getKey();
                toastr.success('Successfully saved value: "' + _this.currentValue().name + '" in key: "' + _this.currentKey() + '"!');
            }).fail(function (r) {
                console.dir(r);
                toastr.error('Could not save the value ' + _this.currentValue().name + 'for key ' + _this.currentKey());
            }).always(function () {
                _this.currentValue('');
            });
        };
        ViewModel.prototype.isValid = function (value) {
            return value !== undefined && value !== '';
        };
        ViewModel.prototype.handleKeyChange = function (key) {
            this.currentKey(key);
            this.getKey();
        };
        ViewModel.prototype.setCurrentValue = function (item) {
            if(item.IsNew) {
                item.KeyName = this.currentKey();
                item.Name = this.newValue();
                this.currentValue(new kvf.KeyValueViewModelFactory().create(item));
            } else {
                this.currentValue(item);
            }
        };
        ViewModel.prototype.getServers = function () {
            var _this = this;
            $.when(dataservice.getServers()).then(function (result) {
                _this.servers.map(result, sm.ServerModel);
            });
        };
        ViewModel.prototype.getKey = function () {
            var _this = this;
            this.isLoading(true);
            this.fadeInContainer(false);
            this.keys([]);
            this.values([]);
            var query = {
                serverName: this.serverName(),
                subKey: this.currentKey() === undefined ? "" : this.currentKey()
            };
            $.when(dataservice.getKey(query)).then(function (result) {
                _this.currentKey(result.KeyName);
                _this.fadeInContainer(true);
                _this.keys(result.SubKeys);
                _this.values.mapWithFactory(result.Values, kvf.KeyValueViewModelFactory);
            }).always(function () {
                _this.isLoading(false);
            });
        };
        ViewModel.prototype.getKeyUrlForBreadcrumb = function (keyName) {
            var url = '#/server/';
            $.each(this.breadcrumbs(), function (i, b) {
                if(b === keyName) {
                    url += keyName;
                    return false;
                }
                url += b + '/';
            });
            return url;
        };
        ViewModel.prototype.getKeyUrl = function (keyName) {
            if(this.currentKey() === undefined || this.currentKey() === "") {
                return "#/server/" + this.serverName() + "/" + keyName;
            }
            return "#/server/" + this.serverName() + "/" + this.currentKey() + "/" + keyName;
        };
        return ViewModel;
    })();
    exports.ViewModel = ViewModel;    
})
