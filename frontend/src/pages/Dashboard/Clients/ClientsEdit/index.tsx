import { Button } from "flowbite-react"
import { FieldArray, Form, Formik, FormikProps } from "formik"
import { useRef } from "react"
import { MdDeleteForever } from "react-icons/md"
import { useQuery } from "react-query"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import { DialogSizes } from "../../../../models/DialogSizes"
import { Address, Client } from "../../../../models/api/Client"
import { addClientFormSchema } from "../../../../schemas/clientFormSchema"
import { getClientAddressesByClientId } from "../../../../services/api/clientApi"
import { AddressForm, FormValues } from "../ClientsAdd"

interface ClientsEditProps {
	client: Client
	open: boolean
	handleVisibility: (visibility: boolean) => void
}

export default function ClientsEdit(props: ClientsEditProps) {
	const formRef = useRef<FormikProps<FormValues>>(null)

	/* 	const updateClientMutation = useMutation(updateClientWithAddresses, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("clients-page-clients")
			toast.success("Client updated successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	}) */

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

		/* 		updateClientMutation.mutate({
			client: {
				id: values.id,
				organizationId: 1,
				name: values.name,
				email: values.email,
			},
			clientsAddressesList: clientAddresses,
		}) */
		actions.resetForm()
		props.handleVisibility(false)
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	const clientAddressForm: Array<AddressForm> = []

	const {
		isLoading: isLoadingClientAddresses,
		isError: isErrorClientAddresses,
		error: errorClientAddresses,
		data: clientAddressesResult,
	} = useQuery<Array<Address>, Error>(["clients-page-client-addresses", props.client.id], () =>
		getClientAddressesByClientId(props.client.id || "0")
	)

	clientAddressesResult?.forEach((address) =>
		clientAddressForm.push({ postalCode: address.zipcode, street: address.address })
	)

	return (
		<MyCustomDialog
			title="Edit Client Details"
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<Formik
					innerRef={formRef}
					initialValues={{
						id: props.client.id,
						name: props.client.name,
						email: props.client.email,
						addresses: clientAddressForm,
					}}
					validationSchema={addClientFormSchema}
					onSubmit={onSubmit}
					enableReinitialize
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
						<Button color="dark" size="md" onClick={() => props.handleVisibility(false)}>
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
