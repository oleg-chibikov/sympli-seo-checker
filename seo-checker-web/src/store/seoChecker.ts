import axios, { CancelTokenSource } from "axios";
import { ActionTree, Module, MutationTree } from "vuex";
import { RootState } from ".";

export interface SearchEngineReferencesRequestInfo {
  keywords: string;
  referenceToFind: string;
}

export interface SearchEngineResults {
  searchEngine: string;
  indexesInSearchResults: number[];
  hasError: boolean;
}

export interface SeoCheckerState {
  searchEngineReferencesRequestInfo: SearchEngineReferencesRequestInfo;
  results: SearchEngineResults[];
  isLoading: boolean;
  cancelTokenSource: CancelTokenSource | undefined;
}

export enum SeoCheckerMutationTypes {
  SET_REQUEST_INFO = "SET_REQUEST_INFO",
  SET_RESULTS = "SET_RESULTS",
  SET_IS_LOADING = "SET_IS_LOADING",
  SET_CANCEL_TOKEN_SOURCE = "SET_CANCEL_TOKEN_SOURCE",
  RESET_RESULTS = "RESET_RESULTS",
}

export enum SeoCheckerActionTypes {
  REQUEST_DATA = "REQUEST_DATA",
}

export const mutations: MutationTree<SeoCheckerState> = {
  [SeoCheckerMutationTypes.SET_RESULTS](state, results: SearchEngineResults[]) {
    state.results = results;
  },
  [SeoCheckerMutationTypes.SET_IS_LOADING](state, isLoading: boolean) {
    state.isLoading = isLoading;
  },
  [SeoCheckerMutationTypes.RESET_RESULTS](state) {
    state.results = [];
    state.searchEngineReferencesRequestInfo = {
      keywords: "",
      referenceToFind: "",
    };
  },
  [SeoCheckerMutationTypes.SET_CANCEL_TOKEN_SOURCE](
    state,
    cancelTokenSource: CancelTokenSource | undefined
  ) {
    state.cancelTokenSource = cancelTokenSource;
  },
  [SeoCheckerMutationTypes.SET_REQUEST_INFO](
    state,
    searchEngineReferencesRequestInfo: SearchEngineReferencesRequestInfo
  ) {
    state.searchEngineReferencesRequestInfo = searchEngineReferencesRequestInfo;
  },
};

export const actions: ActionTree<SeoCheckerState, RootState> = {
  async [SeoCheckerActionTypes.REQUEST_DATA](
    { commit, state },
    searchEngineReferencesRequestInfo: SearchEngineReferencesRequestInfo
  ) {
    const { keywords, referenceToFind } = searchEngineReferencesRequestInfo;

    if (state.cancelTokenSource && state.cancelTokenSource.cancel) {
      state.cancelTokenSource.cancel();
    }
    commit(
      SeoCheckerMutationTypes.SET_REQUEST_INFO,
      searchEngineReferencesRequestInfo
    );
    commit(SeoCheckerMutationTypes.SET_IS_LOADING, true);
    const cancelTokenSource = axios.CancelToken.source();
    commit(SeoCheckerMutationTypes.SET_CANCEL_TOKEN_SOURCE, cancelTokenSource);
    const uri = `${process.env.VUE_APP_SEO_CHECKER_API_URI}/SearchEngineReferences/${keywords}/${referenceToFind}`;
    try {
      const response = await axios.get<SearchEngineResults[]>(uri, {
        cancelToken: cancelTokenSource.token,
      });
      commit(SeoCheckerMutationTypes.SET_RESULTS, response.data);
      commit(SeoCheckerMutationTypes.SET_IS_LOADING, false);
      commit(SeoCheckerMutationTypes.SET_CANCEL_TOKEN_SOURCE, undefined);
    } catch (err) {
      if (axios.isCancel(err)) {
        console.log(`Cancelled: ${uri}`);
        // if request is cancelled it means that there is another one. No need to hide the spinner in this case
      } else {
        console.error(`Cannot get ${uri}: ${err}`);
        alert(err);
        commit(SeoCheckerMutationTypes.SET_IS_LOADING, false);
      }
    }
  },
};

const SeoChecker: Module<SeoCheckerState, RootState> = {
  namespaced: true,
  state: {
    results: [],
    isLoading: false,
    cancelTokenSource: undefined,
    searchEngineReferencesRequestInfo: {
      keywords: "e-settlements",
      referenceToFind: "sympli.com.au",
    },
  },
  mutations: mutations,
  actions: actions,
};

export default SeoChecker;
