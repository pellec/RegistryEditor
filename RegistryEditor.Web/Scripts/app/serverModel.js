define(["require", "exports"], function(require, exports) {
    var ServerModel = (function () {
        function ServerModel(item) {
            this.name = ko.observable(item.Name);
            this.url = ko.observable('#/server/' + item.Name);
        }
        return ServerModel;
    })();
    exports.ServerModel = ServerModel;    
})
