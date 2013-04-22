    export import kv = module('keyValueViewModel');

export class KeyValueViewModelFactory {
    create(item: any): kv.BaseKeyValueViewModel {
        
        if (item.ValueKind === 1) {
            return new kv.StringKeyValueViewModel(item);
        }

        if (item.ValueKind === 7) {
            return new kv.MultiStringKeyValueViewModel(item);
        }

        throw Error('Key value kind could not be mapped to a view model!');
    }
}