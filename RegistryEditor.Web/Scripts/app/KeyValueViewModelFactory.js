define(["require", "exports", 'keyValueViewModel'], function(require, exports, __kv__) {
    var kv = __kv__;

    var KeyValueViewModelFactory = (function () {
        function KeyValueViewModelFactory() { }
        KeyValueViewModelFactory.prototype.create = function (item) {
            if(item.ValueKind === 1) {
                return new kv.StringKeyValueViewModel(item);
            }
            if(item.ValueKind === 7) {
                return new kv.MultiStringKeyValueViewModel(item);
            }
            throw Error('Key value kind could not be mapped to a view model!');
        };
        return KeyValueViewModelFactory;
    })();
    exports.KeyValueViewModelFactory = KeyValueViewModelFactory;    
})
