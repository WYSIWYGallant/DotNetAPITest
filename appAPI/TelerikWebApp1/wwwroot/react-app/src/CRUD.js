import React, { useEffect, useState } from "react";

function App() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formData, setFormData] = useState({ id: "", name: "", email: "" });
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
        if (isEditing) {
            setData((prev) =>
                prev.map((item) =>
                    item.id === formData.id ? { ...item, ...formData } : item
                )
            );
        } else {
            setData((prev) => [...prev, { ...formData, id: Date.now() }]);
        }
        setFormData({ ID: "", Name: "", Age: "",PersonTypeID:"" });
        setIsEditing(false);
    };

    // Handle delete operation
    const handleDelete = (id) => {
        setData((prev) => prev.filter((item) => item.id !== id));
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
                    type="number"
                    name="ID"
                    value={formData.ID}
                    onChange={handleInputChange}
                    placeholder="ID"
                    required
                />
                <input
                    type="text"
                    name="Name"
                    value={formData.Name}
                    onChange={handleInputChange}
                    placeholder="Name"
                    required
                />
                <input
                    type="number"
                    name="Age"
                    value={formData.Age}
                    onChange={handleInputChange}
                    placeholder="Age"
                    required
                />
                <input
                    type="number"
                    name="PersonTypeID"
                    value={formData.PersonTypeID}
                    onChange={handleInputChange}
                    placeholder="PersonTypeID"
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
                            <th style={{ border: "1px solid black", padding: "8px" }}>ID</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Name</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Age</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Person Type</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((item) => (
                            <tr key={item.id}>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.ID}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.Name}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.Age}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.PersonTypeID}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>
                                    <button onClick={() => handleEdit(item)}>Edit</button>
                                    <button onClick={() => handleDelete(item.id)}>Delete</button>
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
