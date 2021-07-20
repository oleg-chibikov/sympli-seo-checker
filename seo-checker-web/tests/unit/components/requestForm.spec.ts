describe("RequestForm.vue", () => {
  // TODO: Quasar library which is used in the project needs to be registered in tests. However, there is no easy way of doing this currently (all the available guides are suitable for Vue2 with createLocalVue).
  // When there is a way of integrating it the following tests should be uncommented.
  // Alternatively, we could wrap the Quasar components into custom components and do the testing, but it seems inpractical.

  it("Initial data is loaded from store", () => {
    // // Arrange
    // const keywords = "a";
    // const referenceToFind = "b";
    // const store = createMockStoreWithIsInitialData(keywords, referenceToFind);
    // // Act
    // const wrapper = shallowMount(RequestForm, store);
    // // Assert
    // expect(wrapper.find(".keywords-input").text()).toBe(keywords);
    // expect(wrapper.find(".keywords-reference").text()).toBe(referenceToFind);
  });

  it("Initial data is empty when missing in store", () => {
    // TODO
    // setup the store so that the searchEngineReferencesRequestInfo is undefined
    // check that the input fields are empty
  });

  it("Submit button click executes REQUEST_DATA action", () => {
    // TODO
    // create a store with jest.fn() instead of the REQUEST_DATA action
    // simulate a click of submit button
    // verify that it has been called once
  });

  it("Reset button click erases input data", () => {
    // TODO
    // setup the store so that the searchEngineReferencesRequestInfo has values
    // simulate a  click of reset button
    // check that the input fields are empty
  });

  it("Reset button click executes RESET_RESULTS mutation", () => {
    // TODO
    // create a store with jest.fn() instead of the RESET_RESULTS mutation
    // simulate a click of reset button
    // verify that it has been called once
  });
});
