define(["require", "exports", 'viewModel'], function(require, exports, __vm__) {
    var vm = __vm__;

    var Bootstrapper = (function () {
        function Bootstrapper() { }
        Bootstrapper.prototype.run = function () {
            ko.applyBindings(new vm.ViewModel());
        };
        return Bootstrapper;
    })();
    exports.Bootstrapper = Bootstrapper;    
})
