import { shallowMount } from "@vue/test-utils";
import { Store } from "vuex";

import Results from "@/components/Results.vue";
import ResultsWrapper from "@/components/ResultsWrapper.vue";
import Spinner from "@/components/Spinner.vue";
import { RootState } from "@/store/index";

import { createMockStore } from "./storeHelper";

const createMockStoreWithIsInitialData = (
  isLoading: boolean
): { global: { plugins: [Store<RootState>] } } => {
  return createMockStore((state) => (state.isLoading = isLoading));
};

describe("ResultsWrapper.vue", () => {
  it("Spinner is when loading", () => {
    // Arrange
    const store = createMockStoreWithIsInitialData(true);

    // Act
    const wrapper = shallowMount(ResultsWrapper, store);

    // Assert
    expect(wrapper.findComponent(Spinner).exists()).toBeTruthy;
    expect(wrapper.findComponent(Results).exists()).toBeFalsy();
  });

  it("Results is displayed when not loading", () => {
    // Arrange
    const store = createMockStoreWithIsInitialData(false);

    // Act
    const wrapper = shallowMount(ResultsWrapper, store);

    // Assert
    expect(wrapper.findComponent(Spinner).exists()).toBeFalsy();
    expect(wrapper.findComponent(Results).exists()).toBeTruthy();
  });
});
