import axios from "axios";

import {
  initialState,
  mutations,
  SeoCheckerMutationTypes,
  SeoCheckerState,
} from "@/store/seochecker";

import { sampleRequest, sampleResponse } from "../sampleData";

let state: SeoCheckerState;

beforeEach(() => {
  state = initialState;
});

describe("SeoCheckerStore.Mutations", () => {
  it("SET_RATE adds results", () => {
    // Arrange, Act
    mutations[SeoCheckerMutationTypes.SET_RESULTS](state, sampleResponse);

    // Assert
    expect(state.results).toBe(sampleResponse);
  });

  it("SET_IS_LOADING sets isLoading", () => {
    // Arrange, Act
    mutations[SeoCheckerMutationTypes.SET_IS_LOADING](state, true);

    // Assert
    expect(state.isLoading).toBe(true);
  });

  it("SET_REQUEST_INFO sets requestInfo", () => {
    // Arrange, Act
    mutations[SeoCheckerMutationTypes.SET_REQUEST_INFO](state, sampleRequest);

    // Assert
    expect(state.searchEngineReferencesRequestInfo).toBe(sampleRequest);
  });

  it("SET_CANCEL_TOKEN sets cancelToken", () => {
    // Arrange, Act
    const cancelTokenSource = axios.CancelToken.source();
    mutations[SeoCheckerMutationTypes.SET_CANCEL_TOKEN_SOURCE](
      state,
      cancelTokenSource
    );

    // Assert
    expect(state.cancelTokenSource).toBe(cancelTokenSource);
  });
});
