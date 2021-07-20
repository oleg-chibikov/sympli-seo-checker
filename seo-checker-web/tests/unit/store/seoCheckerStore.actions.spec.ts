import {
  actions,
  GetSearchEngineReferencesUri,
  initialState,
  SearchEngineReferencesRequestInfo,
  SeoCheckerActionTypes,
  SeoCheckerMutationTypes,
  SeoCheckerState,
} from "@/store/seoChecker";
let state: SeoCheckerState;
import axios from "axios";
import MockAdapter from "axios-mock-adapter";

import { sampleResponse } from "../sampleData";

const keywords = "e-settlements";
const referenceToFind = "sympli.com.au";
const commit = jest.fn();
const mockAxios = new MockAdapter(axios);
const uriToMock = GetSearchEngineReferencesUri(keywords, referenceToFind);
mockAxios.onGet(uriToMock).reply(200, sampleResponse);
const requestDataAction = actions[SeoCheckerActionTypes.REQUEST_DATA] as any;

beforeEach(() => {
  state = initialState;
  commit.mockReset();
});

describe("SeoCheckerStore.Actions", () => {
  it("REQUEST_DATA sets and resets isLoading", async () => {
    // Arrange
    // Act
    await requestDataAction({ commit, state }, { keywords, referenceToFind });

    // Assert
    expect(commit).toHaveBeenCalledWith(
      SeoCheckerMutationTypes.SET_IS_LOADING,
      true
    );
    expect(commit).toHaveBeenCalledWith(
      SeoCheckerMutationTypes.SET_IS_LOADING,
      false
    );
  });

  it("REQUEST_DATA sets data", async () => {
    // Arrange
    // Act
    await requestDataAction({ commit, state }, { keywords, referenceToFind });

    // Assert
    const payload = commit.mock.calls.filter(
      (call) => call[0] === SeoCheckerMutationTypes.SET_RESULTS
    )[0][1];
    expect(payload).toStrictEqual(sampleResponse);
  });

  it("REQUEST_DATA sets requestInfo", async () => {
    // Arrange
    // Act
    await requestDataAction({ commit, state }, { keywords, referenceToFind });

    // Assert
    const payload = commit.mock.calls.filter(
      (call) => call[0] === SeoCheckerMutationTypes.SET_REQUEST_INFO
    )[0][1] as SearchEngineReferencesRequestInfo;
    expect(payload.keywords).toBe(keywords);
    expect(payload.referenceToFind).toBe(referenceToFind);
  });

  // TODO: tests for cancellation: make mock execute for unlimited time for a first call and make a second call while the first is being executed. Verify that the first call is cancelled
});
