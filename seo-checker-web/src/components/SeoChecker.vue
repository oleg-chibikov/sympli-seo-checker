<template>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form class="q-gutter-md" @submit="onSubmit" @reset="onReset">
      <q-input
        v-model="keywords"
        filled
        label="Search keywords *"
        hint="What you would typically type into Google's search bar"
        lazy-rules
        :rules="[
          (val) =>
            (val && val.length > 0) || 'Please fill in the search keywords',
        ]"
      ></q-input>

      <q-input
        v-model="referenceToFind"
        filled
        label="Reference to find *"
        hint="The text to search for in the found web addresses"
        lazy-rules
        :rules="[
          (val) =>
            (val && val.length > 0) || 'Please fill in the reference to find',
        ]"
      ></q-input>

      <div>
        <q-btn label="Check" type="submit" color="primary"></q-btn>
        <q-btn
          label="Reset"
          type="reset"
          color="primary"
          flat
          class="q-ml-sm"
        ></q-btn>
      </div>
    </q-form>

    <transition mode="out-in" name="fade">
      <Spinner v-if="isLoading" />
      <Results v-else />
    </transition>
  </div>
</template>

<script lang="ts">
import {
  SearchEngineReferencesRequestInfo,
  SeoCheckerActionTypes,
  SeoCheckerMutationTypes,
} from "@/store/seoChecker";
import { Options, Vue } from "vue-class-component";
import { namespace } from "vuex-class";
import Results from "./Results.vue";
import Spinner from "./Spinner.vue";

const seoChecker = namespace("seoChecker");
@Options({
  components: { Results, Spinner },
})
export default class SeoChecker extends Vue {
  @seoChecker.State("searchEngineReferencesRequestInfo")
  searchEngineReferencesRequestInfo!: SearchEngineReferencesRequestInfo;

  @seoChecker.Action(SeoCheckerActionTypes.REQUEST_DATA)
  requestData!: (payload: {
    keywords: string;
    referenceToFind: string;
  }) => void;

  @seoChecker.Mutation(SeoCheckerMutationTypes.RESET_RESULTS)
  resetResults!: () => void;

  @seoChecker.State("isLoading") isLoading!: boolean;

  keywords = "";
  referenceToFind = "";

  onSubmit(): void {
    this.requestData({
      keywords: this.keywords,
      referenceToFind: this.referenceToFind,
    });
  }

  onReset(): void {
    this.keywords = "";
    this.referenceToFind = "";
    this.resetResults();
  }

  created(): void {
    this.keywords = this.searchEngineReferencesRequestInfo.keywords;
    this.referenceToFind =
      this.searchEngineReferencesRequestInfo.referenceToFind;
    if (this.keywords.length && this.referenceToFind.length)
      this.requestData({
        keywords: this.keywords,
        referenceToFind: this.referenceToFind,
      });
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
