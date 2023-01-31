import { Button } from "flowbite-react"
import { FieldArray, Form, Formik, FormikProps } from "formik"
import { useRef } from "react"
import toast from "react-hot-toast"
import { MdDeleteForever } from "react-icons/md"
import { useMutation } from "react-query"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import { DialogSizes } from "../../../../models/DialogSizes"
import { Address } from "../../../../models/api/Client"
import { addClientFormSchema } from "../../../../schemas/clientFormSchema"
import { addClientWithAddresses } from "../../../../services/api/clientApi"
import { queryClient } from "../../../../services/queryClient"

interface ClientsAddProps {
	open: boolean
	handleVisibility: (visibility: boolean) => void
}

export interface AddressForm {
	street: string
	postalCode: string
}

export interface FormValues {
	id: string
	name: string
	email: string
	addresses: Array<AddressForm>
}

export default function ClientsAdd(props: ClientsAddProps) {
	const formRef = useRef<FormikProps<FormValues>>(null)

	const addClientMutation = useMutation(addClientWithAddresses, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("clients-page-clients")
			toast.success("Client inserted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = (values: FormValues, actions: any) => {
		const clientAddresses: Array<Address> = []

		values.addresses.forEach((address) => {
			clientAddresses.push({
				id: Date.now().toString(),
				address: address.street,
				clientId: values.id,
				organizationId: 1,
				zipcode: address.postalCode,
			})
		})

		addClientMutation.mutate({
			client: {
				id: values.id,
				organizationId: 1,
				name: values.name,
				email: values.email,
			},
			clientsAddressesList: clientAddresses,
		})
		actions.resetForm()
		props.handleVisibility(false)
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	return (
		<MyCustomDialog
			title="Add a new client"
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<Formik
					innerRef={formRef}
					initialValues={{
						id: "",
						name: "",
						email: "",
						addresses: [{ street: "", postalCode: "" }],
					}}
					validationSchema={addClientFormSchema}
					onSubmit={onSubmit}
				>
					{({ values }) => (
						<Form>
							<div className="px-5">
								<div className="grid gap-10">
									<div className="grid gap-4">
										<MyInput label="Id" name="id" type="text" placeholder="Id" />
										<MyInput label="Name" name="name" type="text" placeholder="Name" />
										<MyInput label="Email" name="email" type="text" placeholder="Email" />
										<FieldArray name="addresses">
											{({ insert, remove, push }) => (
												<div>
													{values.addresses.length > 0 &&
														values.addresses.map((address, index) => (
															<div className="flex items-center mb-3" key={index}>
																<div className="grid grid-cols-2 gap-2">
																	<div className="col-span-1">
																		<MyInput
																			label="Street"
																			name={`addresses.${index}.street`}
																			type="text"
																			placeholder="Street"
																		/>
																	</div>
																	<div className="col-span-1">
																		<MyInput
																			label="Address"
																			name={`addresses.${index}.postalCode`}
																			type="text"
																			placeholder="Address"
																		/>
																	</div>
																</div>
																<div>
																	<button
																		type="button"
																		className="flex pt-3 justify-center items-center my-auto"
																		onClick={() => values.addresses.length > 1 && remove(index)}
																	>
																		<MdDeleteForever fontSize={20} />
																	</button>
																</div>
															</div>
														))}

													<p
														className="cursor-pointer text-xs font-semibold text-gray-500 mt-6"
														onClick={() => push({ street: "", postalCode: "" })}
													>
														Add more addresses
													</p>
												</div>
											)}
										</FieldArray>
									</div>
								</div>
							</div>
						</Form>
					)}
				</Formik>
			}
			actions={
				<div>
					<div className="flex flex-row-reverse gap-2">
						<Button color="dark" size="md" onClick={handleSubmit}>
							Save
						</Button>
						<Button color="dark" size="md" onClick={() => props.handleVisibility(false)}>
							Cancel
						</Button>
					</div>
				</div>
			}
			size={DialogSizes.SMALL}
		/>
	)
}
