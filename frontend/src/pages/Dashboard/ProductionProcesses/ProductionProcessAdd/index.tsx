import { Button } from "flowbite-react"
import { FieldArray, Form, Formik, FormikProps } from "formik"
import { useRef } from "react"
import toast from "react-hot-toast"
import { MdDeleteForever } from "react-icons/md"
import { useMutation } from "react-query"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import MySelect from "../../../../components/controls/MySelect"
import { DialogSizes } from "../../../../models/DialogSizes"
import { Stage } from "../../../../models/api/Stage"
import { addProductionProcessFormSchema } from "../../../../schemas/productionProcessFormSchema"
import { addStagesModel } from "../../../../services/api/stagesModelApi"
import { queryClient } from "../../../../services/queryClient"

interface ProductionProcessAddProps {
	stages: Array<Stage>
	open: boolean
	handleVisibility: (visibility: boolean) => void
}

interface StagesFormValues {
	stageName: string
}

interface FormValues {
	stagesModelName: string
	stagesModelId: string
	stages: Array<StagesFormValues>
}

export default function ProductionProcessAdd(props: ProductionProcessAddProps) {
	const formRef = useRef<FormikProps<FormValues>>(null)

	const addProductionProcessMutation = useMutation(addStagesModel, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("productionProcesses-page-stagesModel")
			toast.success("Production process inserted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const stagesId: string[] = []
	props.stages.forEach((elem) => {
		stagesId.push(elem.id.toString())
	})

	const onSubmit = (values: FormValues, actions: any) => {
		props.handleVisibility(false)
		actions.resetForm()

		const stagesAux: any = []

		values.stages.forEach((stage, index) => {
			stagesAux.push({ id: stage.stageName, position: index })
		})

		addProductionProcessMutation.mutate({
			stagesModel: {
				id: values.stagesModelId,
				organizationId: 1,
				stagesModelName: values.stagesModelName,
			},
			stages: stagesAux,
		})
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	return (
		<MyCustomDialog
			title="Add a new production process"
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<Formik
					innerRef={formRef}
					initialValues={{ stagesModelId: "", stagesModelName: "", stages: [{ stageName: "" }] }}
					validationSchema={() => addProductionProcessFormSchema(stagesId)}
					onSubmit={onSubmit}
				>
					{({ values }) => (
						<Form>
							<div className="px-5">
								<div className="grid gap-10">
									<div className="grid gap-4">
										<MyInput
											label="Id"
											name="stagesModelId"
											type="text"
											placeholder="Production Process Id"
										/>

										<MyInput
											label="Name"
											name="stagesModelName"
											type="text"
											placeholder="Production Process Name"
										/>

										<FieldArray name="stages">
											{({ insert, remove, push }) => (
												<div>
													{values.stages.length > 0 &&
														values.stages.map((stage, index) => (
															<div className="flex items-center mb-3" key={index}>
																<div className="w-full">
																	<MySelect label="Stage" name={`stages.${index}.stageName`}>
																		<option disabled value="">
																			Please select a stage
																		</option>
																		{props.stages.map((option) => (
																			<option key={option.id} value={option.id}>
																				{option.stageName}
																			</option>
																		))}
																	</MySelect>
																</div>
																<div>
																	<button
																		type="button"
																		className="flex pt-3 justify-center items-center my-auto"
																		onClick={() => values.stages.length > 1 && remove(index)}
																	>
																		<MdDeleteForever fontSize={20} />
																	</button>
																</div>
															</div>
														))}

													<p
														className="cursor-pointer text-xs font-semibold text-gray-500 mt-6"
														onClick={() => push({ stageName: "" })}
													>
														Add more stages
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
