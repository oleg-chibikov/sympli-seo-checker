import { createStore } from "vuex";
import VuexPersistence from "vuex-persist";
import seoChecker from "./seoChecker";

export interface RootState {
  author: string;
}

const vuexLocal = new VuexPersistence<RootState>({
  storage: window.localStorage,
});

export default createStore<RootState>({
  state: {
    author: "Oleg Chibikov",
  },
  modules: {
    seoChecker: seoChecker,
  },
  plugins: [vuexLocal.plugin],
});
