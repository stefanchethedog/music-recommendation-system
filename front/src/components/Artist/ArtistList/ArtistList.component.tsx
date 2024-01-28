import { FC } from "react";
import classNames from "classnames";
import { ArtistProps } from "../ArtistCard/ArtistCard.component";
import Artist from "../ArtistCard";

import './ArtistList.styles.scss';

type ArtistlistProps = {
  title: string;
  wrap?: boolean;
  className?: string;
  artistsData?: Omit<ArtistProps, "className">[];
};

const ArtistList: FC<ArtistlistProps> = ({
  className: classes,
  title,
  artistsData,
  wrap = true,
}) => {
  const className = classNames("artist-list", classes);
  return (
    <div className={className}>
      <h2 className="artist-list__title">{title}</h2>
      <div
        className={`artist-list__artists artist-list__artists${
          wrap ? "--wrap" : "--no-wrap"
        }`}
      >
        {artistsData &&
          artistsData.map((artist, index) => {
            return <Artist className="user-list__users__user" {...artist} />;
          })}
      </div>
    </div>
  );
};

export default ArtistList;
