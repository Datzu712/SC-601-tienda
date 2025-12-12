import { ToastManager } from './modules/toast.js';
import { DataFetcher } from './modules/api.js';

window.App = {
    toast: ToastManager.getInstance(document.getElementById('toast-container')),
    _testFetcher: new DataFetcher({ userId: 1, someCrazyFilters: {} }, {
        queryFn: async (state) => {
            console.log('state', state);
            await new Promise((r) => setTimeout(r, 1000));
            throw new Error('Simulated fetch error');
            
            return {
                data: `Fetched data for state: ${JSON.stringify(state)}`
            };
        },
        onSuccess: (res) => {
            console.log('Fetch successful:', res);
        },
        onError: (err) => {
            console.error('Fetch error:', err);
        },
    })
}