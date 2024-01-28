import { FC, Dispatch, SetStateAction } from "react";
import { TextField, Button } from "@mui/material";
import { useForm } from "react-hook-form";
import axios from "axios";
import useSnackbar from "../../Hooks/useSnackbar";

import { GET_USER_BY_USERNAME } from "../../endpoints";

import "./UsernamePage.styles.scss";

type FormData = {
  id: String;
  username: String;
};

interface IUsernamePage {
  setUsername: Dispatch<SetStateAction<String | null>>;
  setId: Dispatch<SetStateAction<String | null>>;
}

const UsernamePage: FC<IUsernamePage> = ({ setUsername, setId }) => {
  const { createSnackbar } = useSnackbar({
    message: `User signed succesfully`,
    errorMessage: "User doesn't exist",
  });

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();

  const onSubmit = handleSubmit(async (data) => {
    await axios
      .get(GET_USER_BY_USERNAME(data.username))
      .then((res) => {
        setUsername(res.data.username);
        setId(res.data.id);
        createSnackbar({ error: false });
      })
      .catch((err) => {
        console.log(err);
        createSnackbar({ error: true });
      });
  });

  return (
    <div className="container">
      <form className="container__form" onSubmit={onSubmit}>
        <h1 className="container__form__title">Enter your username</h1>
        <div className="container__form__field-wrapper">
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
