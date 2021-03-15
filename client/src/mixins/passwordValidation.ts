import { helpers } from "vuelidate/lib/validators";

const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/;
export const validPassword = helpers.regex("validPassword", passwordRegex);
