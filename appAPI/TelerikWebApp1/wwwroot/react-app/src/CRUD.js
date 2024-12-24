import React, { useEffect, useState } from "react";

function App() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formData, setFormData] = useState({ personID: 0, personName: "", personAge: "", personType:0});
    const [isEditing, setIsEditing] = useState(false);

    // Fetch data from the API
    useEffect(() => {
        fetch("https://localhost:7203/api/Person")
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Failed to fetch data");
                }
                return response.json();
            })
            .then((data) => {
                setData(data);
                setLoading(false);
            })
            .catch((err) => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    // Handle input changes for the form
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    // Handle create or update operation
    const handleSubmit = (e) => {
        e.preventDefault();
        const person = {
            personID: formData.personID,
            personName: formData.personName,
            personAge: formData.personAge,
            personType: formData.personType
        };

        if (isEditing) {
            fetch(`https://localhost:7203/api/Person/${person.personID}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(person),
            })
                .then((response) => {
                    if (!response.ok) {
                        throw new Error("Failed to update data");
                    }
                })
                .catch((err) => {
                    setError(err.message);
                });
            setData((prev) =>
                prev.map((item) =>
                    item.personID === formData.personID ? { ...item, ...formData } : item
                )
            );
        } else {
            fetch("https://localhost:7203/api/Person", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(person),
            })
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Failed to create person");
                }
                return response.json();
            })
            .then((newPerson) => {
                setData((prev) => [...prev, newPerson]);
            })
            .catch((err) => {
                setError(err.message);
            });
        }
        setFormData({ personID: 0, personName: "", personAge: "", personType:0 });
        setIsEditing(false);
    };

    // Handle delete operation
    const handleDelete = (id) => {
        fetch(`https://localhost:7203/api/Person/${id}`, {
            method: "DELETE",
        })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Failed to delete data");
            }
            setData((prev) => prev.filter((item) => item.personID !== id));
        })
        .catch((err) => {
            setError(err.message);
        });
    };

    // Handle edit operation
    const handleEdit = (item) => {
        setFormData(item);
        setIsEditing(true);
        
    };

    return (
        <div style={{ padding: "20px", fontFamily: "Arial, sans-serif" }}>
            <h1>CRUD Operations Example</h1>

            {/* Create/Edit Form */}
            <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>

                <input
                    type="text"
                    name="personName"
                    value={formData.personName}
                    onChange={handleInputChange}
                    placeholder="Name"
                    required
                />
                <input
                    type="number"
                    name="personAge"
                    value={formData.personAge}
                    onChange={handleInputChange}
                    placeholder="Age"
                    required
                />
                <input
                    type="number"
                    name="personType"
                    value={formData.personType}
                    onChange={handleInputChange}
                    placeholder="Person Type"
                    required
                />
                <button type="submit">{isEditing ? "Update" : "Create"}</button>
            </form>

            {/* Loading, Error, or Data Table */}
            {loading && <p>Loading data...</p>}
            {error && <p style={{ color: "red" }}>Error: {error}</p>}

            {!loading && !error && (
                <table
                    style={{
                        border: "1px solid black",
                        borderCollapse: "collapse",
                        width: "100%",
                    }}
                >
                    <thead>
                        <tr>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Name</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Age</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Person Type</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((item) => (
                            <tr key={item.personID}>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.personName}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.personAge}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.personType}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>
                                    <button onClick={() => handleEdit(item)}>Edit</button>
                                    <button onClick={() => handleDelete(item.personID)}>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default App;
