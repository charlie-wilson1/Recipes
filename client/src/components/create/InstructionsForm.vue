<template>
	<section class="instructions-form">
		<b-form-group
			:class="{
				'form-group--error': $v._instruction.description.$error,
			}"
		>
			<label for="instructions">Instructions</label>
			<div>
				<b-input-group>
					<b-form-textarea
						type="text"
						v-model="$v._instruction.description.$model"
						id="instructions"
						placeholder="Start typing instructions"
					></b-form-textarea>
					<b-input-group-append>
						<b-button @click="addInstruction" :disabled="$v.$invalid"
							>Add
						</b-button>
					</b-input-group-append>
				</b-input-group>
			</div>
			<div
				class="error"
				v-if="
					!$v._instruction.description.required &&
						$v._instruction.description.$error
				"
			>
				Field is required
			</div>
		</b-form-group>
	</section>
</template>

<script lang="ts">
import { Component, PropSync, Vue } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import CreateList from "@/components/create/CreateList.vue";
import { defaultInstruction } from "@/models/DefaultModels";

@Component({
	components: {
		CreateList,
	},
	mixins: [validationMixin],
})
export default class InstructionsForm extends Vue {
	@PropSync("instruction", { required: true })
	@Validate({
		description: { required },
	})
	_instruction = { ...defaultInstruction };

	addInstruction() {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fill out required fields.");
			return;
		}

		this._instruction.orderNumber = this.$store.getters.highestInstructionOrderNumber;
		this.$store.dispatch("insertInstruction", this._instruction);
		this.$v.$reset();
		this._instruction = { ...defaultInstruction };
	}

	beforeCreate() {
		this.$store.dispatch("createDefaultInstruction");
	}
}
</script>

<style lang="scss" scoped>
.instructions-form {
}
</style>
