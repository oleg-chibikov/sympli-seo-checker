import { shallowMount } from "@vue/test-utils";
import { Store } from "vuex";

import Results from "@/components/Results.vue";
import SearchEngineResultsCard from "@/components/SearchEngineResultsCard.vue";
import { RootState } from "@/store/index";
import { SearchEngineResults } from "@/store/seoChecker";

import { sampleResponse } from "../sampleData";
import { createMockStore } from "./storeHelper";

const createMockStoreWithIsInitialData = (
  results?: SearchEngineResults[]
): { global: { plugins: [Store<RootState>] } } => {
  return createMockStore((state) => (state.results = results));
};

describe("Results.vue", () => {
  it("Displays SearchEngineResultsCard for each result", () => {
    // Arrange
    const store = createMockStoreWithIsInitialData(sampleResponse);

    // Act
    const wrapper = shallowMount(Results, store);

    // Assert
    expect(wrapper.findAllComponents(SearchEngineResultsCard)).toHaveLength(
      sampleResponse.length
    );
  });

  it("Displays nothing when there are no results", () => {
    // Arrange
    const store = createMockStoreWithIsInitialData(undefined);

    // Act
    const wrapper = shallowMount(Results, store);

    // Assert
    expect(wrapper.find("div")).toBeNull;
  });

  it("Alternates colors for tiles", () => {
    // Arrange
    const store = createMockStoreWithIsInitialData(sampleResponse);

    // Act
    const wrapper = shallowMount(Results, store);

    // Assert
    expect(wrapper.findAllComponents(SearchEngineResultsCard)).toHaveLength(3);
    const components = wrapper.findAllComponents(SearchEngineResultsCard);
    expect(components[0].classes()).not.toEqual(components[1].classes());
    expect(components[0].classes()).toEqual(components[2].classes());
  });
});
