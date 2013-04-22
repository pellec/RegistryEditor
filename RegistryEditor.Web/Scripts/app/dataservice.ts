/// <reference path="../typings/jquery.d.ts" />
declare var $: JQueryStatic;

export class DataService {

    getKey(query) {
        var params = 'serverName=' + query.serverName;
        if(query.subKey !== '') {
            params += '&subKey=' + query.subKey; 
        }

        return $.getJSON('api/registry', params);
    }

    getServers() {
        return $.getJSON('api/server');
    }

    saveValue(data: any) {
        return $.post('api/registryvalue', data);
    }

    saveKey(data: any) {
        return $.post('api/registry', data);
    }
}