describe("SearchEngineResultsCard.vue", () => {
  // TODO: Quasar library which is used in the project needs to be registered in tests. However, there is no easy way of doing this currently (all the available guides are suitable for Vue2 with createLocalVue).
  // When there is a way of integrating it the following tests should be uncommented.
  // Alternatively, we could wrap the Quasar components into custom components and do the testing, but it seems inpractical.
  it("Displays header", () => {
    // TODO:
    // pass props to the component and render it
    // check if the header contains the searchEngine
  });

  it("Displays badge for every number in results", () => {
    // TODO:
    // pass props to the component and render it
    // check if the badge is displayed n times (n = results.indexesInSearchResults.length)
  });

  it("Adds bg-negative class to the results which have errors", () => {
    // TODO:
    // pass props to the component with hasErrors = true and render it
    // check if the section component has the bg-negative class
  });
});
