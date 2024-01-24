import { FC, Dispatch, SetStateAction } from "react";
import { TextField, Button } from "@mui/material";
import { useForm } from "react-hook-form";

import "./UsernamePage.styles.scss";

type FormData = {
  username: String;
};

interface IUsernamePage {
  setUsername: Dispatch<SetStateAction<String | null>>;
}

const UsernamePage: FC<IUsernamePage> = ({ setUsername }) => {
  const {
    register,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();
  const onSubmit = handleSubmit((data) => setUsername(data.username));

  return (
    <div className="container">
      <form className="container__form" onSubmit={onSubmit}>
        <h1 className="container__form__title">Enter your username</h1>
        <div className = "container__form__field-wrapper">
          <TextField
            label="Username"
            className="container__form__text-field"
            variant="filled"
            {...register("username", { required: true })}
          />
        </div>
        <button className="container__form__button">SUBMIT</button>
      </form>
    </div>
  );
};

export default UsernamePage;
