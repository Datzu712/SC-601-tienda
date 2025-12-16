/**
 * Strongly inspired by Tanstack Query
 * @see https://tanstack.com/query/v5/docs/framework/react/reference/useQuery
 *
 * @typedef {Object} QueryOptions - Options for configuring the data fetch query.
 * @property {Function} queryFn - The function to fetch data.
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
        if (!options || typeof options.queryFn !== 'function') {
            throw new Error('A valid queryFn function must be provided in options.');
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
            ...options
        };

        this.#previousOriginalState = state;
        this.data = new Proxy(state, {
            set: (target, prop, value, receiver) => {
                // avoid unnecessary updates
                if (value && JSON.stringify(value) === JSON.stringify(receiver)) {
                    console.debug('DataFetcher: No changes detected in state, skipping fetch.');
                    return true;
                }

                this.fetchData(value).catch(() => undefined);
                this.#previousOriginalState = value;
                return Reflect.set(target, prop, value, receiver);
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
                this.#options.onSuccess(res);
            }
            
            return res;
        } catch (e) {
            if (this.#options.retry && retryCount <= this.#options.retry) {
                console.debug('Retrieving data from state', retryCount);
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
}
