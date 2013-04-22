define(['ko', 'jquery'], function(ko, $) {

    ko.bindingHandlers.triggerSmoothFade = {
        init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor()();
            if (value) {
                $(element).fadeIn('fast');
            } else {
                $(element).fadeOut(10);
            }
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor()();
            if (value) {
                $(element).fadeIn('fast');
            } else {
                $(element).fadeOut(10);
            }
        }
    };

    ko.bindingHandlers.modal = {
        init: function () {
            ko.bindingHandlers.with.init.apply(this, arguments);
        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var returnValue = ko.bindingHandlers.with.update.apply(this, arguments);
            
            if(value) {
                $(element).modal('show');
            } else {
                $(element).modal('hide');
            }

            return returnValue;
        }
    };
    
    ko.observableArray.fn.map = function (data, Constructor) {
        var mapped = ko.utils.arrayMap(data, function (item) {
            return new Constructor(item);
        });

        this(mapped);

        return this;
    };

    ko.observableArray.fn.mapWithFactory = function (data, Factory) {
        var mapped = ko.utils.arrayMap(data, function (item) {
            return new Factory().create(item);
        });

        this(mapped);

        return this;
    };
});