import {
	DndContext,
	MouseSensor,
	TouchSensor,
	closestCenter,
	useSensor,
	useSensors,
} from "@dnd-kit/core"
import { restrictToVerticalAxis } from "@dnd-kit/modifiers"
import { SortableContext, arrayMove, verticalListSortingStrategy } from "@dnd-kit/sortable"
import { Button } from "flowbite-react"
import { FieldArray, Form, Formik, FormikProps } from "formik"
import { useRef } from "react"
import toast from "react-hot-toast"
import { MdDeleteForever } from "react-icons/md"
import { useMutation, useQuery } from "react-query"
import { DraggableDropDown } from "../../../../components/DraggableDropDown"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import { DialogSizes } from "../../../../models/DialogSizes"
import { Stage } from "../../../../models/api/Stage"
import { StagesModel } from "../../../../models/api/StagesModel"
import { StagesModelStagePaginated } from "../../../../models/api/StagesModelStage"
import { editProductionProcessFormSchema } from "../../../../schemas/productionProcessFormSchema"
import { updateStagesModel } from "../../../../services/api/stagesModelApi"
import { getStagesModelStageByStageModelId } from "../../../../services/api/stagesModelStageApi"
import { queryClient } from "../../../../services/queryClient"

interface ProductionProcessEditProps {
	stages: Array<Stage>
	stageModel: StagesModel
	open: boolean
	handleVisibility: (visibility: boolean) => void
}

interface StagesFormValues {
	id: string
	stageId: string
}

interface FormValues {
	stagesModelName: string
	stagesModelId: string
	stages: Array<StagesFormValues>
}

export default function ProductionProcessEdit(props: ProductionProcessEditProps) {
	const sensors = useSensors(
		useSensor(TouchSensor, {
			activationConstraint: {
				delay: 250,
				tolerance: 5,
			},
		}),
		useSensor(MouseSensor, {
			activationConstraint: {
				delay: 250,
				tolerance: 5,
			},
		})
	)
	const formRef = useRef<FormikProps<FormValues>>(null)

	const {
		isLoading: isLoadingByStageModelId,
		isError: isErrorByStageModelId,
		error: errorByStageModelId,
		data: stagesByStageModelId,
	} = useQuery<StagesModelStagePaginated, Error>(
		["productionProcesses-page-stagesByStageModelId", props.stageModel.id],
		() => getStagesModelStageByStageModelId(props.stageModel.id || "0")
	)

	const stagesId: string[] = []
	props.stages.forEach((elem) => {
		stagesId.push(elem.id.toString())
	})

	const stagesModelById: Array<StagesFormValues> = []

	stagesByStageModelId?.results.forEach((stage, index) => {
		stagesModelById.push({
			id: (index + 1).toString(),
			stageId: stage.stagesId,
		})
	})

	function handleDragEnd(event: any) {
		const { active, over } = event
		if (active.id !== over.id) {
			if (formRef.current) {
				const activeIndex = formRef.current.values.stages.findIndex((item) => item.id === active.id)
				const overIndex = formRef.current.values.stages.findIndex((item) => item.id === over.id)

				formRef.current.setFieldValue(
					"stages",
					arrayMove(formRef.current.values.stages, activeIndex, overIndex)
				)
			}
		}
	}

	const updateProductionProcessMutation = useMutation(updateStagesModel, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("productionProcesses-page-stagesByStageModelId")
			queryClient.invalidateQueries("productionProcesses-page-stagesModel")

			toast.success("Production process updated successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = async (values: FormValues, actions: any) => {
		actions.resetForm()

		const stagesAux: any = []

		values.stages.forEach((stage, index) => {
			stagesAux.push({ id: stage.stageId, position: index + 1 })
		})

		updateProductionProcessMutation.mutate({
			stagesModel: {
				id: values.stagesModelId,
				organizationId: 1,
				stagesModelName: values.stagesModelName,
			},
			stages: stagesAux,
		})

		props.handleVisibility(false)
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	return (
		<MyCustomDialog
			title="Edit production process"
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<Formik
					innerRef={formRef}
					initialValues={{
						stagesModelId: props.stageModel.id,
						stagesModelName: props.stageModel.stagesModelName,
						stages: stagesModelById,
					}}
					validationSchema={() => editProductionProcessFormSchema(stagesId)}
					onSubmit={onSubmit}
					enableReinitialize
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
											disabled
											placeholder="Production Process Id"
										/>

										<MyInput
											label="Name"
											name="stagesModelName"
											type="text"
											placeholder="Production Process Name"
										/>

										<div>
											<label className="block text-sm font-medium text-gray-700">Stages</label>
											<div className="relative mt-1 rounded-md shadow-sm">
												<DndContext
													collisionDetection={closestCenter}
													onDragEnd={handleDragEnd}
													sensors={sensors}
													modifiers={[restrictToVerticalAxis]}
												>
													<SortableContext
														items={values.stages}
														strategy={verticalListSortingStrategy}
													>
														<FieldArray name="stages">
															{({ insert, remove, push }) => (
																<div>
																	{values.stages.length > 0 &&
																		values.stages.map((stage, index) => (
																			<div className="flex items-center mb-3" key={stage.id}>
																				<div className="w-full">
																					<DraggableDropDown
																						selectName={`stages.${index}.stageId`}
																						id={stage.id}
																						selectOptions={props.stages}
																					/>
																				</div>
																				<div className="h-full flex justify-center items-center mb-2">
																					<button
																						type="button"
																						onClick={() =>
																							values.stages.length > 1 && remove(index)
																						}
																					>
																						<MdDeleteForever fontSize={20} />
																					</button>
																				</div>
																			</div>
																		))}

																	<p
																		className="cursor-pointer text-xs font-semibold text-gray-500"
																		onClick={() =>
																			push({
																				id: (values.stages.length + 1).toString(),
																				stageId: "",
																			})
																		}
																	>
																		Add more stages
																	</p>
																</div>
															)}
														</FieldArray>
													</SortableContext>
												</DndContext>
											</div>
										</div>
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
