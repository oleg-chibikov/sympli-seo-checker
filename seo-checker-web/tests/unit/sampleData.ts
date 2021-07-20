import { SearchEngineResults } from "@/store/seoChecker";

export const sampleResponse: SearchEngineResults[] = [
  {
    searchEngine: "Google",
    indexesInSearchResults: [2],
    hasError: false,
  },
  {
    searchEngine: "Bing",
    indexesInSearchResults: [2, 4],
    hasError: false,
  },
  {
    searchEngine: "Yandex",
    indexesInSearchResults: [2, 4],
    hasError: true,
  },
];

export const sampleRequest = {
  keywords: "hello",
  referenceToFind: "world",
};
