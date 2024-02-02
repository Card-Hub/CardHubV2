// import { useFetch } from "#app"

export const useApi: typeof useFetch = (path, options = {}) => {
    const config = useRuntimeConfig();
    options.baseURL = config.public.baseURL;
    return useFetch(path, options)
}