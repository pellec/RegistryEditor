(function () {

    var root = this;

    config();
    definePlugins();
    
    requirejs([
            'ko.extensions'
    ], boot);

    function definePlugins() {
        define('jquery', [], function () { return root.jQuery; });
        define('ko', [], function () { return root.ko; });
        define('sammy', [], function () { return root.jQuery.sammy; });
        define('_', [], function () { return root._; });
    }

    function config() {
        requirejs.config({
            baseUrl: 'Scripts/app'
        });
    }

    function boot() {
        require(['bootstrapper'], function(bs) {
                (new bs.Bootstrapper()).run();
        });
    }
})();