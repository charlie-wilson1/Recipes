<template>
	<section class="instructions-form">
		<b-form-row>
			<b-col md="6">
				<b-form-group
					:class="{
						'form-group--error': $v.currentInstruction.description.$error,
					}"
				>
					<label for="instructions">Instructions</label>
					<div>
						<b-input-group>
							<b-form-textarea
								type="text"
								v-model="currentInstruction.description"
								id="instructions"
								placeholder="Start typing instructions"
							></b-form-textarea>
							<b-input-group-append>
								<b-button
									@click="addInstruction(currentInstruction)"
									:disabled="$v.$invalid"
									>Add
								</b-button>
							</b-input-group-append>
						</b-input-group>
					</div>
					<div
						class="error"
						v-if="
							!$v.currentInstruction.description.required &&
								$v.currentInstruction.description.$error
						"
					>
						Field is required
					</div>
				</b-form-group>
			</b-col>
			<b-col v-if="instructions">
				<CreateList
					:values="getSimpleStringForCreateList(instructions)"
					editable="true"
					deletable="true"
					:handle-edit="handleEdit"
					:handle-delete="handleDelete"
					:handle-move="moveItem"
				/>
			</b-col>
		</b-form-row>
	</section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { Validate } from "vuelidate-property-decorators";
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import CreateList from "@/components/create/CreateList.vue";
import { Instruction } from "@/models/RecipeModels";
import { defaultInstruction } from "@/models/DefaultModels";

@Component({
	components: {
		CreateList,
	},
	mixins: [validationMixin],
})
export default class InstructionsForm extends Vue {
	@Prop({
		required: false,
		default: [],
	})
	instructions?: Array<Instruction>;

	@Validate({
		description: { required },
	})
	get currentInstruction(): Instruction {
		let instruction: Instruction = this.$store.getters.selectedInstruction;

		if (!instruction) {
			instruction = { ...defaultInstruction };
		}

		return instruction;
	}

	addInstruction(instruction: Instruction) {
		this.$v.$touch();

		if (this.$v.$invalid) {
			Vue.$toast.error("Please fill out required fields.");
			return;
		}

		if (!this.instructions) {
			this.instructions = [];
		}

		if (instruction.orderNumber !== 0) {
			if ((this.instructions ?? []).length < instruction.orderNumber) {
				return;
			}

			const payload = {
				instruction: instruction,
				index: instruction.orderNumber - 1,
			};

			this.$store.dispatch("updateInstruction", payload);
		} else {
			instruction.orderNumber =
				this.instructions.length === 0 ? 1 : this.instructions.length + 1;

			this.$store.dispatch("insertInstruction", instruction);
		}

		this.$store.dispatch("createDefaultInstruction");
		this.$v.$reset();
	}

	getSimpleStringForCreateList(defaultValues: Array<Instruction>) {
		return defaultValues.map(val => {
			return {
				defaultValue: val.description,
			};
		});
	}

	handleEdit(index: number) {
		if ((this.instructions ?? []).length < index) {
			Vue.$toast.error("Could not find selected instruction.");
			return;
		}

		this.$store.dispatch("setSelectedInstruction", index);
	}

	handleDelete(index: number) {
		if ((this.instructions ?? []).length < index) {
			Vue.$toast.error("Could not find selected instruction.");
			return;
		}

		this.$store.dispatch("removeInstruction", index);
	}

	moveItem(from: number, to: number) {
		if (!this.instructions) {
			Vue.$toast.error("Could not find selected instruction.");
			return;
		}

		const swappedItem = this.instructions[to];
		const item = this.instructions.splice(from, 1)[0];
		item.orderNumber = to;
		swappedItem.orderNumber = from;
		this.instructions.splice(to, 0, item);
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
