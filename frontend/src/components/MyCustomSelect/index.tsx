import Select from "react-select";
import makeAnimated from "react-select/animated";
import "./style.scss";
import { useField } from "formik";

const animatedComponents = makeAnimated();

export interface Option {
  value: string;
  label: string;
}

interface MyCustomSelectProps {
  name: string;
  options: Array<Option>;
  isMulti: boolean;
}

export const MyCustomSelect = (props: MyCustomSelectProps) => {
  const [field, meta, helpers] = useField(props.name);

  return (
    <>
      <Select
        name={props.name}
        value={field.value}
        components={animatedComponents}
        isMulti={props.isMulti}
        onChange={(value) => helpers.setValue(value)}
        options={props.options}
        onBlur={() => helpers.setTouched(true)}
        className={
          meta.touched && meta.error
            ? "MyCustomSelectContainer transition duration-500 block w-full rounded-md border-red-400 focus:border-red-500 focus:ring-red-500 sm:text-sm"
            : "MyCustomSelectContainer transition duration-500 block w-full rounded-md border-gray-300 focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
        }
        classNamePrefix="MyCustomSelect"
      />
      {meta.touched && meta.error && (
        <p className="block text-xs font-medium text-red-500">{meta.error}</p>
      )}
    </>
  );
};
