var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", 'dataservice'], function(require, exports, __ds__) {
    var ds = __ds__;

    var dataservice = new ds.DataService();
    var BaseKeyValueViewModel = (function () {
        function BaseKeyValueViewModel(item) {
            this.keyName = item.KeyName;
            this.name = item.Name;
            this.valueKind = item.ValueKind;
        }
        BaseKeyValueViewModel.prototype.templateName = function () {
            return "";
        };
        BaseKeyValueViewModel.prototype.save = function () {
            console.dir(this);
        };
        return BaseKeyValueViewModel;
    })();
    exports.BaseKeyValueViewModel = BaseKeyValueViewModel;    
    var StringKeyValueViewModel = (function (_super) {
        __extends(StringKeyValueViewModel, _super);
        function StringKeyValueViewModel(item) {
                _super.call(this, item);
            this.value = ko.observable(item.Value);
            this.valueDescription = "Value is type string";
        }
        StringKeyValueViewModel.prototype.templateName = function () {
            return "stringvalue-template";
        };
        StringKeyValueViewModel.prototype.getData = function () {
            return {
                keyName: this.keyName,
                valueName: this.name,
                value: this.value(),
                valueKind: 1
            };
        };
        return StringKeyValueViewModel;
    })(BaseKeyValueViewModel);
    exports.StringKeyValueViewModel = StringKeyValueViewModel;    
    var MultiStringKeyValueViewModel = (function (_super) {
        __extends(MultiStringKeyValueViewModel, _super);
        function MultiStringKeyValueViewModel(item) {
                _super.call(this, item);
            this.values = ko.observableArray([]);
            this.valueDescription = "Value is type string array";
            this.init(item);
        }
        MultiStringKeyValueViewModel.prototype.init = function (dto) {
            var values = ko.utils.arrayMap(dto.Value, function (v) {
                return new StringValueModel(v);
            });
            this.values(values);
        };
        MultiStringKeyValueViewModel.prototype.templateName = function () {
            return "multistringvalue-template";
        };
        MultiStringKeyValueViewModel.prototype.getData = function () {
            return {
                keyName: this.keyName,
                valueName: this.name,
                value: ko.utils.arrayMap(this.values(), function (v) {
                    return v.value();
                }),
                valueKind: 7
            };
        };
        MultiStringKeyValueViewModel.prototype.addValue = function () {
            this.values.push(new StringValueModel());
        };
        return MultiStringKeyValueViewModel;
    })(BaseKeyValueViewModel);
    exports.MultiStringKeyValueViewModel = MultiStringKeyValueViewModel;    
    var StringValueModel = (function () {
        function StringValueModel(value) {
            this.value = value ? ko.observable(value) : ko.observable('');
        }
        return StringValueModel;
    })();    
})
