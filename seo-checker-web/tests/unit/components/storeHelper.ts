import { createStore, Module, Store } from "vuex";

import { RootState } from "@/store";
import { initialState, SeoCheckerState } from "@/store/seoChecker";

export const createMockStore = (
  modifyState: ((state: SeoCheckerState) => void) | undefined = undefined,
  modifyModule:
    | ((store: Module<SeoCheckerState, RootState>) => void)
    | undefined = undefined
): { global: { plugins: [Store<RootState>] } } => {
  const state = initialState;
  modifyState && modifyState(state);
  const seoCheckerModule: Module<SeoCheckerState, RootState> = {
    namespaced: true,
    state,
  };
  modifyModule && modifyModule(seoCheckerModule);
  const store = createStore<RootState>({
    modules: {
      seoChecker: seoCheckerModule,
    },
  });
  return {
    global: {
      plugins: [store],
    },
  };
};
