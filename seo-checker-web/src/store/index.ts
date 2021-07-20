import { createStore } from "vuex";
import VuexPersistence from "vuex-persist";

import seoChecker, { SeoCheckerState } from "./seoChecker";

export interface RootState {
  author: string;
  seoChecker?: SeoCheckerState;
}

const vuexLocal = new VuexPersistence<RootState>({
  storage: window.localStorage,
  reducer: (state) => ({
    seoChecker: {
      searchEngineReferencesRequestInfo:
        state.seoChecker?.searchEngineReferencesRequestInfo,
    },
  }),
});

export default createStore<RootState>({
  state: {
    author: "Oleg Chibikov",
    seoChecker: undefined,
  },
  modules: {
    seoChecker: seoChecker,
  },
  plugins: [vuexLocal.plugin],
});
