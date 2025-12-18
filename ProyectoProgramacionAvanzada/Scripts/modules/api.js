/**
 * Strongly inspired by Tanstack Query
 * @see https://tanstack.com/query/v5/docs/framework/react/reference/useQuery
 *
 * @typedef {Object} QueryOptions - Options for configuring the data fetch query.
 * @property {Function} queryFn - The function to fetch data.
 * @property {string} endpoint - The endpoint to fetch data from (then queryFn would be not necessary).
 * @property {Function} onSuccess - Callback function on successful data fetch.
 * @property {Function} onError - Callback function on data fetch error.
 * @property {number | false} [retry=5] - Whether to retry on failure.
 * @property {boolean} [fetchOnMount=true] - Fetch data when document is loaded.
 * @property {boolean} [fetchOnWindowFocus=true] - Fetch data when window gains focus.
 * @property {number} [retryDelay=1000] - Delay between retries in milliseconds.
 * @property {boolean} [enabled=true] - Whether the query should automatically run.
 */

/**
 * @template T
 */
export class DataFetcher {
    /**
     * @type {QueryOptions}
     */
    #options;

    /**
     * @type {T | null}
     */
    data = null; // todo: change name to state

    /**
     * It is used to pass states without proxy to queryFn so we can avoid infinite loops
     * @type {T}
     */
    #previousOriginalState = null;


    /**
     * Creates an instance of DataFetcher.
     * @param {T} state - Any object that could be mutated so when it changes the queryFn is re-executed.
     * @param {QueryOptions} options - Configuration options for the data fetch query.
     */
    constructor(state, options) {
        let queryFn = options.queryFn;
        if (options.endpoint) {
            queryFn = this.#abstractQueryFn.bind({}, options.endpoint);
        }
        
        if (!options || typeof queryFn !== 'function') {
            throw new Error('A valid options.queryFn function must be provided in options. Or specify an endpoint string.');
        }
        
        if (options.onSuccess && typeof options.onSuccess !== 'function') {
            throw new Error('onSuccess must be a function if provided.');
        }

        if (options.onError && typeof options.onError !== 'function') {
            throw new Error('onError must be a function if provided.');
        }
        
        this.#options = {
            retry: 5,
            retryDelay: 1000,
            enabled: true,
            fetchOnMount: true,
            fetchOnWindowFocus: true,
            ...options,
            queryFn,
        };

        this.#previousOriginalState = state;
        this.data = new Proxy(state, {
            set: (target, prop, value, receiver) => {
                this.#previousOriginalState = { ...target };
                
                const result = Reflect.set(target, prop, value, receiver);
                
                if (JSON.stringify(target) === JSON.stringify(this.#previousOriginalState)) {
                    console.debug('DataFetcher: No changes detected in state, skipping fetch.');
                    return result;
                }
                this.fetchData(target).catch(() => undefined);

                return result;
            },
        });
        
        this.#setUpEvents();
    }

    /**
     * @param {T} currentState - New query value to trigger data fetch.
     * @param {number} [retryCount=0] - Current retry attempt count. You can disable retries by using options.retry = 0.
     * @returns {Promise<any>} - Promise resolving to fetched data.
     */
    async fetchData(currentState, retryCount = 0) {
        try {
            const res = await this.#options.queryFn(currentState);
            
            if (this.#options.onSuccess) {
                console.debug('DataFetcher: Triggering onSuccess callback.');
                this.#options.onSuccess(res);
            }
            
            return res;
        } catch (e) {
            if (this.#options.retry && retryCount <= this.#options.retry) {
                console.debug('Retrieving data from state', retryCount);
                
                await new Promise((r) => setTimeout(r, this.#options.retryDelay));
                
                return await this.fetchData(currentState, ++retryCount);
            } else {
                console.error('DataFetcher: Max retries reached or retries disabled.', e);
            }
            
            
            if (this.#options.onError) {
                this.#options.onError(e);
            }
            throw e;
        }
    }

    #setUpEvents() {
        console.log(this.#options)
        if (!this.#options.enabled) return;
        
        if (this.#options.fetchOnWindowFocus) {
            window.addEventListener('focus', () => {
                this.fetchData(this.#previousOriginalState).catch(() => undefined);
            });
        }
        
        if (this.#options.fetchOnMount) {
            document.addEventListener('DOMContentLoaded', () => {
                this.fetchData(this.#previousOriginalState).catch(() => undefined);
            });
        }
    }

    /**
     * 
     * @param { string } endpoint - HTTP endpoint to send the request to.
     * @param { T } currentState - Current state to be sent in the request body.
     */
    async #abstractQueryFn(endpoint, currentState) {
        const params = new URLSearchParams(currentState).toString();
        
        const response = await fetch(`${endpoint}?${params}`, {
            method: 'GET',
        });

        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.statusText}`);
        }

        return await response.json();
    }
        
    
    // todo: add method detroy to remove event listeners
}
