define(["require", "exports"], function(require, exports) {
    var DataService = (function () {
        function DataService() { }
        DataService.prototype.getKey = function (query) {
            var params = 'serverName=' + query.serverName;
            if(query.subKey !== '') {
                params += '&subKey=' + query.subKey;
            }
            return $.getJSON('api/registry', params);
        };
        DataService.prototype.getServers = function () {
            return $.getJSON('api/server');
        };
        DataService.prototype.saveValue = function (data) {
            return $.post('api/registryvalue', data);
        };
        DataService.prototype.saveKey = function (data) {
            return $.post('api/registry', data);
        };
        return DataService;
    })();
    exports.DataService = DataService;    
})
